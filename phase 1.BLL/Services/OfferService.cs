using phase_1.BLL.DTOs;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;

namespace phase_1.BLL.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;

        public OfferService(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<OfferDTO> CreateOfferAsync(CreateOfferDTO dto)
        {
            // ✅ إصلاح: تحقق من Pending فقط (Status=1)
            // لو الطالب عنده offer مرفوض أو ملغي يقدر يعمل عرض جديد
            var existing = await _offerRepository.GetByStudentAndCaseAsync(
                dto.StudentUserId, dto.CaseId, pendingOnly: true);

            if (existing != null)
                throw new InvalidOperationException("لديك عرض قيد المراجعة على هذه الحالة بالفعل.");

            var offer = new Offer
            {
                CaseId = dto.CaseId,
                StudentUserId = dto.StudentUserId,
                Message = dto.Message,
                ProposedPrice = dto.ProposedPrice,
                EstimatedSessionsCount = dto.EstimatedSessionsCount,
                Status = 1,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _offerRepository.AddAsync(offer);

            return new OfferDTO
            {
                Id = created.Id,
                CaseId = created.CaseId,
                StudentUserId = created.StudentUserId,
                Message = created.Message,
                ProposedPrice = created.ProposedPrice,
                EstimatedSessionsCount = created.EstimatedSessionsCount,
                Status = created.Status,
                CreatedAt = created.CreatedAt
            };
        }

        public async Task<OfferDetailsDTO?> GetOfferDetailsAsync(int id)
        {
            var offer = await _offerRepository.GetByIdAsync(id);

            if (offer == null)
                return null;

            return new OfferDetailsDTO
            {
                Id = offer.Id,
                CaseId = offer.CaseId,
                CaseTitle = offer.Case?.Title,
                StudentUserId = offer.StudentUserId,
                StudentName = offer.StudentUser?.FullName,
                Message = offer.Message,
                ProposedPrice = offer.ProposedPrice,
                EstimatedSessionsCount = offer.EstimatedSessionsCount,
                Status = offer.Status,
                CreatedAt = offer.CreatedAt,
                DecidedAt = offer.DecidedAt
            };
        }

        public async Task<IEnumerable<OfferDTO>> GetOffersForCaseAsync(int caseId)
        {
            var offers = await _offerRepository.GetOffersByCaseIdAsync(caseId);

            return offers.Select(o => new OfferDTO
            {
                Id = o.Id,
                CaseId = o.CaseId,
                StudentUserId = o.StudentUserId,
                Message = o.Message,
                ProposedPrice = o.ProposedPrice,
                EstimatedSessionsCount = o.EstimatedSessionsCount,
                Status = o.Status,
                CreatedAt = o.CreatedAt
            });
        }

        public async Task<IEnumerable<OfferDTO>> GetOffersByStudentAsync(int studentId)
        {
            var offers = await _offerRepository.GetOffersByStudentIdAsync(studentId);

            return offers.Select(o => new OfferDTO
            {
                Id = o.Id,
                CaseId = o.CaseId,
                StudentUserId = o.StudentUserId,
                Message = o.Message,
                ProposedPrice = o.ProposedPrice,
                EstimatedSessionsCount = o.EstimatedSessionsCount,
                Status = o.Status,
                CreatedAt = o.CreatedAt
            });
        }

        public async Task<bool> RejectOfferAsync(int offerId)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId);

            if (offer == null)
                return false;

            if (offer.Status != 1)
                return false;

            offer.Status = 3;
            offer.DecidedAt = DateTime.UtcNow;

            await _offerRepository.UpdateAsync(offer);
            return true;
        }

        // ✅ إصلاح: بترجع Pending فقط عشان الطالب يقدر يعمل عرض جديد بعد الرفض
        public async Task<OfferDTO?> GetExistingOfferAsync(int studentId, int caseId)
        {
            var offer = await _offerRepository.GetByStudentAndCaseAsync(
                studentId, caseId, pendingOnly: true);

            if (offer == null)
                return null;

            return new OfferDTO
            {
                Id = offer.Id,
                CaseId = offer.CaseId,
                StudentUserId = offer.StudentUserId,
                Message = offer.Message,
                ProposedPrice = offer.ProposedPrice,
                EstimatedSessionsCount = offer.EstimatedSessionsCount,
                Status = offer.Status,
                CreatedAt = offer.CreatedAt
            };
        }
    }
}