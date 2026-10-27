
namespace RecruitingATS.Models;

public class InterviewFeedback
{
    public int Id { get; set; }

    public int InterviewId { get; set; }

    public int InterviewerId { get; set; }

    public int Rating { get; set; }

    public string Strengths { get; set; } = "";

    public string Concerns { get; set; } = "";

    public string Recommendation { get; set; } = "";

    public DateTime SubmittedAt { get; set; }

    public Interview Interview { get; set; } = null!;
}
