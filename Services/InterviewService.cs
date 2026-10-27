
using Microsoft.EntityFrameworkCore;
using RecruitingATS.Data;
using RecruitingATS.Models;
using RecruitingATS.ViewModels;

namespace RecruitingATS.Services;

public class InterviewService : IInterviewService
{
    private readonly ApplicationDbContext _db;

    public InterviewService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<bool> ScheduleAsync(
        ScheduleInterviewViewModel model,
        int recruiterEmployeeId)
    {
        if (model.SelectedEmployeeIds.Count == 0)
            return false;

        if (model.SelectedEmployeeIds.Distinct().Count()
            != model.SelectedEmployeeIds.Count)
            return false;

        if (model.LeadEmployeeId.HasValue &&
            !model.SelectedEmployeeIds.Contains(
                model.LeadEmployeeId.Value))
            return false;

        if (model.ScheduledAt <= DateTime.Now)
            return false;

        // TODO: Verify the application and confirm that
        // every selected employee belongs to the
        // application's requisition interviewer team.
        // Also verify the recruiter's permission and scope.

        var interview = new Interview
        {
            ApplicationId = model.ApplicationId,
            ScheduledAt = model.ScheduledAt,
            Type = model.Type,
            Mode = model.Mode,
            Status = "Scheduled"
        };

        foreach (var employeeId in
                 model.SelectedEmployeeIds.Distinct())
        {
            interview.PanelMembers.Add(
                new InterviewPanelMember
                {
                    EmployeeId = employeeId,
                    IsLead = model.LeadEmployeeId == employeeId
                });
        }

        _db.Interviews.Add(interview);

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<List<Interview>> GetMyInterviewsAsync(
        int interviewerEmployeeId)
    {
        return await _db.Interviews
            .Where(i => i.PanelMembers.Any(
                p => p.EmployeeId == interviewerEmployeeId))
            .Include(i => i.PanelMembers)
            .Include(i => i.Feedbacks)
            .OrderBy(i => i.ScheduledAt)
            .ToListAsync();
    }

    public async Task<bool> SubmitFeedbackAsync(
        SubmitFeedbackViewModel model,
        int interviewerEmployeeId)
    {
        var interview = await _db.Interviews
            .FirstOrDefaultAsync(i => i.Id == model.InterviewId);

        if (interview == null)
            return false;

        var isAssigned = await _db.InterviewPanelMembers
            .AnyAsync(p =>
                p.InterviewId == model.InterviewId &&
                p.EmployeeId == interviewerEmployeeId);

        if (!isAssigned)
            return false;

        var alreadySubmitted = await _db.InterviewFeedbacks
            .AnyAsync(f =>
                f.InterviewId == model.InterviewId &&
                f.InterviewerId == interviewerEmployeeId);

        if (alreadySubmitted)
            return false;

        if (model.Rating < 1 || model.Rating > 5)
            return false;

        var feedback = new InterviewFeedback
        {
            InterviewId = model.InterviewId,
            InterviewerId = interviewerEmployeeId,
            Rating = model.Rating,
            Strengths = model.Strengths,
            Concerns = model.Concerns,
            Recommendation = model.Recommendation,
            SubmittedAt = DateTime.UtcNow
        };

        _db.InterviewFeedbacks.Add(feedback);

        await _db.SaveChangesAsync();

        return true;
    }
}
