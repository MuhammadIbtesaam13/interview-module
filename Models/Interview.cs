
namespace RecruitingATS.Models;

public class Interview
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }

    public DateTime ScheduledAt { get; set; }

    public string Type { get; set; } = "";

    public string Mode { get; set; } = "";

    public string Status { get; set; } = "Scheduled";

    public ICollection<InterviewPanelMember> PanelMembers { get; set; }
        = new List<InterviewPanelMember>();

    public ICollection<InterviewFeedback> Feedbacks { get; set; }
        = new List<InterviewFeedback>();
}
