using System.Linq;
using Chamedoon.Application.Services.Admin.Countries;
using ChamedoonWebUI.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class CountriesController : Controller
{
    private readonly IAdminCountryService _countryService;

    public CountriesController(IAdminCountryService countryService)
    {
        _countryService = countryService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var countriesResult = await _countryService.GetCountriesAsync(cancellationToken);
        if (!countriesResult.IsSuccess || countriesResult.Result is null)
        {
            return Problem(countriesResult.Message);
        }

        var viewModel = new CountriesIndexViewModel
        {
            Countries = countriesResult.Result.Select(CountryPanelViewModel.FromDto).ToList(),
            NewCountry = new CountryEditViewModel()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveCountry(CountryEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "لطفاً خطاهای فرم کشور را بررسی کنید.";
            return RedirectToCountryAnchor(model.Id);
        }

        var result = await _countryService.SaveCountryAsync(model.ToInput(), cancellationToken);
        if (!result.IsSuccess || result.Result is null)
        {
            TempData["Error"] = result.Message;
            return RedirectToCountryAnchor(model.Id);
        }

        TempData["Success"] = model.Id.HasValue ? "اطلاعات کشور به‌روزرسانی شد." : "کشور جدید اضافه شد.";
        return RedirectToCountryAnchor(result.Result.Id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveLivingCost(CountryLivingCostEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "لطفاً مقدار هزینه زندگی را بررسی کنید.";
            return RedirectToCountryAnchor(model.CountryId);
        }

        var result = await _countryService.SaveLivingCostAsync(model.ToInput(), cancellationToken);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
        }
        else
        {
            TempData["Success"] = "هزینه زندگی ذخیره شد.";
        }

        return RedirectToCountryAnchor(model.CountryId);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveRestriction(CountryRestrictionEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "توضیح محدودیت را وارد کنید.";
            return RedirectToCountryAnchor(model.CountryId);
        }

        var result = await _countryService.SaveRestrictionAsync(model.ToInput(), cancellationToken);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
        }
        else
        {
            TempData["Success"] = "محدودیت ثبت شد.";
        }

        return RedirectToCountryAnchor(model.CountryId);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveJob(CountryJobEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "اطلاعات شغل را تکمیل کنید.";
            return RedirectToCountryAnchor(model.CountryId);
        }

        var result = await _countryService.SaveJobAsync(model.ToInput(), cancellationToken);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
        }
        else
        {
            TempData["Success"] = "شغل ذخیره شد.";
        }

        return RedirectToCountryAnchor(model.CountryId);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveEducation(CountryEducationEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "اطلاعات تحصیلی را کامل کنید.";
            return RedirectToCountryAnchor(model.CountryId);
        }

        var result = await _countryService.SaveEducationAsync(model.ToInput(), cancellationToken);
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
        }
        else
        {
            TempData["Success"] = "مسیر تحصیلی ذخیره شد.";
        }

        return RedirectToCountryAnchor(model.CountryId);
    }

    private IActionResult RedirectToCountryAnchor(long? countryId)
    {
        var url = Url.Action(nameof(Index)) ?? "/";
        var anchor = countryId.HasValue ? $"#country-{countryId.Value}" : string.Empty;
        return Redirect(url + anchor);
    }
}
