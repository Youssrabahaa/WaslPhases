using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using phase_1.BLL.DTOs;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories;
using phase_1.DAL.Repositories.Interfaces;

namespace phase_1.BLL.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly INotificationService _notificationService;

        public MatchService(
            IMatchRepository matchRepository,
            IOfferRepository offerRepository,
            INotificationService notificationService)
        {
            _matchRepository = matchRepository;
            _offerRepository = offerRepository;
            _notificationService = notificationService;
        }

        public async Task<bool> AcceptOfferAsync(int offerId)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null || offer.Status != 1) return false;

            var existingMatch = await _matchRepository.GetByOfferIdAsync(offerId);
            if (existingMatch != null) return false;

            var match = new Match
            {
                OfferId = offer.Id,
                CaseId = offer.CaseId,
                PatientUserId = offer.Case.PatientUserId,
                StudentUserId = offer.StudentUserId,
                AcceptedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                Status = 1
            };

            await _matchRepository.AddAsync(match);
            offer.Status = 2;
            offer.DecidedAt = DateTime.UtcNow;
            offer.Case.Status = 2;

            var allCaseOffers = await _offerRepository.GetCaseOffersAsync(offer.CaseId);
            foreach (var other in allCaseOffers)
            {
                if (other.Id != offer.Id && other.Status == 1)
                {
                    other.Status = 3;
                    other.DecidedAt = DateTime.UtcNow;
                }
            }

            await _matchRepository.SaveChangesAsync();

            var conversation = new Conversation
            {
                MatchId = match.Id,
                CreatedAt = DateTime.UtcNow
            };
            await _matchRepository.AddConversationAsync(conversation);
            await _matchRepository.SaveChangesAsync();

            // ✅ إشعار 1: للطالب — تم قبول عرضه
            await _notificationService.SendAsync(
                offer.StudentUserId,
                "تم قبول عرضك",
                $"قبل المريض عرضك على حالة \"{offer.Case.Title}\".",
                type: 1, referenceId: match.Id, referenceType: "Match");

            // ✅ إشعار 2: للمريض — تم إنشاء مطابقة
            await _notificationService.SendAsync(
                offer.Case.PatientUserId,
                "تم إنشاء مطابقة جديدة",
                "تم إنشاء مطابقتك مع الطالب. يمكنك الآن جدولة الجلسات.",
                type: 3, referenceId: match.Id, referenceType: "Match");

            // ✅ إشعار 3: للطالب أيضًا — تم إنشاء مطابقة
            await _notificationService.SendAsync(
                offer.StudentUserId,
                "تم إنشاء مطابقة جديدة",
                "تم إنشاء مطابقتك مع المريض. يمكنك الآن جدولة الجلسات.",
                type: 3, referenceId: match.Id, referenceType: "Match");

            return true;
        }

        public async Task<MatchDetailsDTO?> GetMatchByIdAsync(int matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null) return null;

            return new MatchDetailsDTO
            {
                MatchId = match.Id,
                CaseId = match.CaseId,
                CaseTitle = match.Case.Title,
                Status = match.Status,
                AcceptedAt = match.AcceptedAt,
                PatientUserId = match.PatientUserId,
                StudentUserId = match.StudentUserId,
                PatientName = match.PatientUser.FullName,
                PatientPhone = match.PatientUser.Phone,
                StudentName = match.StudentUser.FullName,
                StudentPhone = match.StudentUser.Phone,
                CaseDescription = match.Case.Description,
                TreatmentCategory = match.Case.TreatmentCategory.Name,
                Governorate = match.Case.Governorate,
                City = match.Case.City,
                Area = match.Case.Area,
                AgreedPrice = match.Offer.ProposedPrice,
                SessionsCount = match.Offer.EstimatedSessionsCount,
                OfferMessage = match.Offer.Message,
                CompletedSessions = match.Sessions.Count(x => x.Status == 3),
                RemainingSessions = (match.Offer.EstimatedSessionsCount ?? 0)
                                    - match.Sessions.Count(x => x.Status == 3)
            };
        }

        public async Task<List<MatchDTO>> GetPatientMatchesAsync(int patientId)
        {
            var matches = await _matchRepository.GetByPatientIdAsync(patientId);
            return matches.Select(x => new MatchDTO
            {
                MatchId = x.Id,
                CaseId = x.CaseId,
                CaseTitle = x.Case.Title,
                Status = x.Status,
                AcceptedAt = x.AcceptedAt
            }).ToList();
        }

        public async Task<List<MatchDTO>> GetStudentMatchesAsync(int studentId)
        {
            var matches = await _matchRepository.GetByStudentIdAsync(studentId);
            return matches.Select(x => new MatchDTO
            {
                MatchId = x.Id,
                CaseId = x.CaseId,
                CaseTitle = x.Case.Title,
                Status = x.Status,
                AcceptedAt = x.AcceptedAt
            }).ToList();
        }

        public async Task<bool> CompleteMatchAsync(int matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null || match.Status != 1) return false;
            if (!match.Sessions.Any() || match.Sessions.Any(x => x.Status != 3)) return false;

            match.Status = 2;
            match.CompletedAt = DateTime.UtcNow;

            await _matchRepository.UpdateAsync(match);
            await _matchRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelMatchAsync(int matchId, int userId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null || match.Status != 1) return false;

            bool isParticipant = match.PatientUserId == userId || match.StudentUserId == userId;
            if (!isParticipant) return false;

            bool treatmentStarted = match.Sessions.Any(s => s.Status == 2 || s.Status == 3);
            if (treatmentStarted) return false;

            match.Status = 3;
            match.Offer.Status = 3;
            match.Offer.DecidedAt = DateTime.UtcNow;
            match.Case.Status = 1;

            await _matchRepository.UpdateAsync(match);
            await _matchRepository.SaveChangesAsync();
            return true;
        }

        public async Task CancelExpiredMatchesAsync()
        {
            var matches = await _matchRepository.GetActiveMatchesAsync();
            foreach (var match in matches)
            {
                if (match.CreatedAt.AddDays(10) <= DateTime.UtcNow && !match.Sessions.Any())
                {
                    match.Status = 3;
                    match.Offer.Status = 3;
                    match.Offer.DecidedAt = DateTime.UtcNow;
                    match.Case.Status = 1;
                }
            }
            await _matchRepository.SaveChangesAsync();
        }
    }
}