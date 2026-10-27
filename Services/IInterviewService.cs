
using RecruitingATS.Models;
using RecruitingATS.ViewModels;

namespace RecruitingATS.Services;

public interface IInterviewService
{
    Task<bool> ScheduleAsync(
        ScheduleInterviewViewModel model,
        int recruiterEmployeeId);

    Task<List<Interview>> GetMyInterviewsAsync(
        int interviewerEmployeeId);

    Task<bool> SubmitFeedbackAsync(
        SubmitFeedbackViewModel model,
        int interviewerEmployeeId);
}
