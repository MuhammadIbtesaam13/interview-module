
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitingATS.Services;
using RecruitingATS.ViewModels;

namespace RecruitingATS.Controllers;

[Authorize]
public class InterviewsController : Controller
{
    private readonly IInterviewService _service;

    public InterviewsController(IInterviewService service)
    {
        _service = service;
    }

    private int? GetEmployeeId()
    {
        // This works only if your app stores the employee ID
        // in the NameIdentifier claim.
        var value = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        return int.TryParse(value, out var id)
            ? id
            : null;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Schedule()
    {
        return View(new ScheduleInterviewViewModel
        {
            ScheduledAt = DateTime.Now.AddDays(1)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Schedule(
        ScheduleInterviewViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var employeeId = GetEmployeeId();

        if (employeeId == null)
            return Forbid();

        var result = await _service.ScheduleAsync(
            model, employeeId.Value);

        if (!result)
        {
            ModelState.AddModelError(
                "",
                "Interview could not be scheduled. Check the details.");

            return View(model);
        }

        TempData["Message"] = "Interview scheduled successfully.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> MyInterviews()
    {
        var employeeId = GetEmployeeId();

        if (employeeId == null)
            return Forbid();

        var interviews = await _service.GetMyInterviewsAsync(
            employeeId.Value);

        return View(interviews);
    }

    [HttpGet]
    public IActionResult Feedback(int id)
    {
        return View(new SubmitFeedbackViewModel
        {
            InterviewId = id
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Feedback(
        SubmitFeedbackViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var employeeId = GetEmployeeId();

        if (employeeId == null)
            return Forbid();

        var result = await _service.SubmitFeedbackAsync(
            model, employeeId.Value);

        if (!result)
        {
            ModelState.AddModelError(
                "",
                "Feedback could not be submitted. Check your assignment and previous submission.");

            return View(model);
        }

        TempData["Message"] = "Feedback submitted successfully.";

        return RedirectToAction(nameof(MyInterviews));
    }
}
