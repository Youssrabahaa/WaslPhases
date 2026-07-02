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

        public MatchService(
            IMatchRepository matchRepository,
            IOfferRepository offerRepository)
        {
            _matchRepository = matchRepository;
            _offerRepository = offerRepository;
        }

        // AcceptOffer
        // Business Rules:
        //   1. Offer must exist and be Pending (Status=1)
        //   2. No existing Match for this Offer (unique)
        //   3. Match.Status = Active (1)
        //   4. Offer.Status = Accepted (2)
        //   5. Case.Status = Matched (2)
        //   6. All other offers on same Case → Rejected (3)
        //   7. A Conversation is created automatically
        public async Task<bool> AcceptOfferAsync(int offerId)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId);

            if (offer == null)
                return false;

            if (offer.Status != 1)
                return false;

            var existingMatch = await _matchRepository.GetByOfferIdAsync(offerId);
            if (existingMatch != null)
                return false;

            var match = new Match
            {
                OfferId = offer.Id,
                CaseId = offer.CaseId,
                PatientUserId = offer.Case.PatientUserId,
                StudentUserId = offer.StudentUserId,
                AcceptedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                Status = 1   // Active
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
                    other.Status = 3;   // Rejected
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

            return true;
        }

        public async Task<MatchDetailsDTO?> GetMatchByIdAsync(int matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);

            if (match == null)
                return null;

            return new MatchDetailsDTO
            {
                MatchId = match.Id,
                CaseId = match.CaseId,
                CaseTitle = match.Case.Title,
                Status = match.Status,
                AcceptedAt = match.AcceptedAt,

                PatientName = match.PatientUser.FullName,
                PatientPhone = match.PatientUser.Phone,
                PatientUserId = match.PatientUserId,

                StudentName = match.StudentUser.FullName,
                StudentPhone = match.StudentUser.Phone,
                StudentUserId = match.StudentUserId,

                CaseDescription = match.Case.Description,
                TreatmentCategory = match.Case.TreatmentCategory.Name,

                Governorate = match.Case.Governorate,
                City = match.Case.City,
                Area = match.Case.Area,

                AgreedPrice = match.Offer.ProposedPrice,
                SessionsCount = match.Offer.EstimatedSessionsCount,
                OfferMessage = match.Offer.Message,
                OfferId = match.OfferId,

                CompletedSessions =
                    match.Sessions.Count(x => x.Status == 3),

                RemainingSessions =
                    (match.Offer.EstimatedSessionsCount ?? 0)
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

        // CompleteMatch
        // Business Rules:
        //   1. Match must exist
        //   2. At least one Session exists
        //   3. Every Session must be Completed (Status=3)
        //   4. Match.Status → Completed (2)
        //   5. Match.CompletedAt = UtcNow
        public async Task<bool> CompleteMatchAsync(int matchId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);

            if (match == null)
                return false;

            if (match.Status != 1)
                return false;

            if (!match.Sessions.Any())
                return false;

            if (match.Sessions.Any(x => x.Status != 3))
                return false;

            match.Status = 2;   // Completed
            match.CompletedAt = DateTime.UtcNow;

            await _matchRepository.UpdateAsync(match);
            await _matchRepository.SaveChangesAsync();

            return true;
        }

        // CancelMatch (Manual)
        // Business Rules:
        //   Allowed ONLY if:
        //     Case 1: No sessions exist at all
        //     Case 2: Sessions exist but ALL are still Scheduled (Status=1)
        //   NOT allowed if any Session is InProgress(2) or Completed(3)
        //   When cancelled:
        //     Match.Status  → Cancelled (3)
        //     Offer.Status  → Cancelled (3)
        //     Case.Status   → Open (1)
        public async Task<bool> CancelMatchAsync(int matchId, int userId)
        {
            var match = await _matchRepository.GetByIdAsync(matchId);

            if (match == null)
                return false;
            if (match.Status != 1)
                return false;

            bool isParticipant =
                match.PatientUserId == userId ||
                match.StudentUserId == userId;

            if (!isParticipant)
                return false;

            bool treatmentStarted = match.Sessions.Any(s =>
                s.Status == 2 ||  // InProgress
                s.Status == 3);   // Completed

            if (treatmentStarted)
                return false; 

            match.Status = 3;   // Cancelled

            match.Offer.Status = 3;   // Cancelled
            match.Offer.DecidedAt = DateTime.UtcNow;

            match.Case.Status = 1;   // Open (available again)

            await _matchRepository.UpdateAsync(match);
            await _matchRepository.SaveChangesAsync();

            return true;
        }

        // CancelExpiredMatches (Automatic - Case 3)
        // Business Rules:
        //   Conditions for auto-cancel:
        //     1. Match is still Active (Status=1)
        //     2. No Session exists
        //     3. 10 days have passed since Match.CreatedAt
        //   When cancelled:
        //     Match.Status → Cancelled (3)
        //     Offer.Status → Cancelled (3)
        //     Case.Status  → Open (1)
        public async Task CancelExpiredMatchesAsync()
        {
            var matches = await _matchRepository.GetActiveMatchesAsync();

            foreach (var match in matches)
            {
                bool hasExpired = match.CreatedAt.AddDays(10) <= DateTime.UtcNow;
                bool hasNoSessions = !match.Sessions.Any();

                if (!hasExpired || !hasNoSessions)
                    continue;

                match.Status = 3;   // Cancelled

                match.Offer.Status = 3;   // Cancelled
                match.Offer.DecidedAt = DateTime.UtcNow;

                match.Case.Status = 1;   // Open
            }

            await _matchRepository.SaveChangesAsync();
        }
    }
}
