using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class OfferController : Controller
{
    public IActionResult BrowseCases()
    {
        return View();
    }

    public IActionResult CreateOffer()
    {
        return View();
    }

    public IActionResult MyOffers()
    {
        return View();
    }

    public IActionResult OfferDetails()
    {
        return View();
    }
}
