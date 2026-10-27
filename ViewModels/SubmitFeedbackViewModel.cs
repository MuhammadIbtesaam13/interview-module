
using System.ComponentModel.DataAnnotations;

namespace RecruitingATS.ViewModels;

public class SubmitFeedbackViewModel
{
    public int InterviewId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    public string Strengths { get; set; } = "";

    [Required]
    public string Concerns { get; set; } = "";

    [Required]
    public string Recommendation { get; set; } = "";
}
