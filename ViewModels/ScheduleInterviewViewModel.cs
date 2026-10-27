
using System.ComponentModel.DataAnnotations;

namespace RecruitingATS.ViewModels;

public class ScheduleInterviewViewModel
{
    [Required]
    public int ApplicationId { get; set; }

    [Required]
    public DateTime ScheduledAt { get; set; }

    [Required]
    public string Type { get; set; } = "";

    [Required]
    public string Mode { get; set; } = "";

    [Required]
    public List<int> SelectedEmployeeIds { get; set; } = new();

    public int? LeadEmployeeId { get; set; }
}
