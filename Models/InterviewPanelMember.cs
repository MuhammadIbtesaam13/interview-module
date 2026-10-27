
namespace RecruitingATS.Models;

public class InterviewPanelMember
{
    public int Id { get; set; }

    public int InterviewId { get; set; }

    public int EmployeeId { get; set; }

    public bool IsLead { get; set; }

    public Interview Interview { get; set; } = null!;
}
