using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers;

public class OfferController : Controller
{
    private readonly IOfferService _offerService;

    public OfferController(IOfferService offerService)
    {
        _offerService = offerService;
    }

    [HttpGet]
    public IActionResult CreateOffer(int caseId)
    {
        var model = new CreateOfferDTO { CaseId = caseId };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOffer(CreateOfferDTO model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        await _offerService.CreateOfferAsync(model);
        return RedirectToAction(nameof(MyOffers));
    }

    public async Task<IActionResult> MyOffers(int studentId = 1)
    {
        var offers = await _offerService.GetOffersByStudentAsync(studentId);
        return View(offers);
    }

    public async Task<IActionResult> OfferDetails(int id)
    {
        var offer = await _offerService.GetOfferDetailsAsync(id);
        if (offer == null) return NotFound();

        return View(offer);
    }

    [HttpPost]
    public async Task<IActionResult> AcceptOffer(int id)
    {
        await _offerService.AcceptOfferAsync(id);
        return RedirectToAction("OfferDetails", new { id });
    }

    [HttpPost]
    public async Task<IActionResult> RejectOffer(int id)
    {
        await _offerService.RejectOfferAsync(id);
        return RedirectToAction("OfferDetails", new { id });
    }
}
