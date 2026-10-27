
using Microsoft.EntityFrameworkCore;
using RecruitingATS.Data;
using RecruitingATS.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC registration
builder.Services.AddControllersWithViews();

// Database registration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Interview service registration
builder.Services.AddScoped<IInterviewService, InterviewService>();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
