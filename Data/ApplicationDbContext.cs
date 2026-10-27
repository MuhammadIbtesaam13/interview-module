
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RecruitingATS.Models;

namespace RecruitingATS.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<Interview> Interviews => Set<Interview>();

    public DbSet<InterviewPanelMember> InterviewPanelMembers
        => Set<InterviewPanelMember>();

    public DbSet<InterviewFeedback> InterviewFeedbacks
        => Set<InterviewFeedback>();
}
