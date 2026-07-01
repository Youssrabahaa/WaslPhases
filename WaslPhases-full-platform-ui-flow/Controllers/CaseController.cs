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
        var patientId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var cases = await _caseService.GetPatientCasesAsync(patientId);
        return View(cases);
    }

    public async Task<IActionResult> CaseDetails(int id)
    {
        var c = await _caseService.GetByIdAsync(id);

        if (c == null)
            return NotFound();

        return View(c);
    }

    public IActionResult CreateCase()
    {
        var patientId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var dto = new CreateCaseDTO { PatientUserId = patientId };
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCase(CreateCaseDTO dto)
    {
        dto.PatientUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

        if (!ModelState.IsValid)
            return View(dto);

        await _caseService.CreateAsync(dto);

        TempData["Success"] = "تم نشر الحالة بنجاح.";
        return RedirectToAction(nameof(MyCases));
    }

    public async Task<IActionResult> EditCase(int id)
    {
        var c = await _caseService.GetByIdAsync(id);

        if (c == null)
            return NotFound();

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
        if (!ModelState.IsValid)
            return View(dto);

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
        var patientId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var result = await _caseService.CloseAsync(id, patientId);

        if (!result)
            TempData["Error"] = "لا يمكن إغلاق هذه الحالة.";
        else
            TempData["Success"] = "تم إغلاق الحالة.";

        return RedirectToAction(nameof(MyCases));
    }
}