using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers;

public class CaseController : Controller
{
    private readonly ICaseService _caseService;

    public CaseController(ICaseService caseService)
    {
        _caseService = caseService;
    }

    public async Task<IActionResult> MyCases()
    {
        if (!IsPatient())
            return RedirectToAction("BrowseCases", "Offer", new { portal = "student" });

        var patientId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var cases = await _caseService.GetPatientCasesAsync(patientId);
        return View(cases);
    }

    public async Task<IActionResult> CaseDetails(int id)
    {
        var c = await _caseService.GetByIdAsync(id);

        if (c == null)
            return NotFound();

        var role = HttpContext.Session.GetInt32("UserRole") ?? 0;
        var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

        if (role == 1 && c.PatientUserId != userId)
            return Forbid();

        if (role == 2 && c.Status != 1)
            return Forbid();

        return View(c);
    }

    public IActionResult CreateCase()
    {
        if (!IsPatient())
            return RedirectToAction("BrowseCases", "Offer", new { portal = "student" });

        var patientId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var dto = new CreateCaseDTO { PatientUserId = patientId };
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCase(CreateCaseDTO dto)
    {
        if (!IsPatient())
            return RedirectToAction("BrowseCases", "Offer", new { portal = "student" });

        dto.PatientUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

        if (!ModelState.IsValid)
            return View(dto);

        await _caseService.CreateAsync(dto);

        TempData["Success"] = "تم نشر الحالة بنجاح.";
        return RedirectToAction(nameof(MyCases));
    }

    public async Task<IActionResult> EditCase(int id)
    {
        if (!IsPatient())
            return RedirectToAction("BrowseCases", "Offer", new { portal = "student" });

        var c = await _caseService.GetByIdAsync(id);

        if (c == null)
            return NotFound();

        var patientId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (c.PatientUserId != patientId)
            return Forbid();

        var dto = new UpdateCaseDTO
        {
            Id = c.Id,
            ServiceTypeId = c.ServiceTypeId,
            TreatmentCategoryId = c.TreatmentCategoryId,
            Title = c.Title,
            Description = c.Description,
            Urgency = c.Urgency,
            EstimatedPriceMin = c.EstimatedPriceMin,
            EstimatedPriceMax = c.EstimatedPriceMax,
            NeedsSupervisorApproval = c.NeedsSupervisorApproval,
            Governorate = c.Governorate,
            City = c.City,
            Area = c.Area
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCase(UpdateCaseDTO dto)
    {
        if (!IsPatient())
            return RedirectToAction("BrowseCases", "Offer", new { portal = "student" });

        if (!ModelState.IsValid)
            return View(dto);

        var c = await _caseService.GetByIdAsync(dto.Id);
        var patientId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (c == null)
            return NotFound();

        if (c.PatientUserId != patientId)
            return Forbid();

        var result = await _caseService.UpdateAsync(dto);

        if (!result)
        {
            TempData["Error"] = "لا يمكن تعديل هذه الحالة.";
            return View(dto);
        }

        TempData["Success"] = "تم تعديل الحالة بنجاح.";
        return RedirectToAction(nameof(CaseDetails), new { id = dto.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CloseCase(int id)
    {
        if (!IsPatient())
            return RedirectToAction("BrowseCases", "Offer", new { portal = "student" });

        var patientId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var result = await _caseService.CloseAsync(id, patientId);

        if (!result)
            TempData["Error"] = "لا يمكن إغلاق هذه الحالة.";
        else
            TempData["Success"] = "تم إغلاق الحالة.";

        return RedirectToAction(nameof(MyCases));
    }

    private bool IsPatient() => HttpContext.Session.GetInt32("UserRole") == 1;
}
