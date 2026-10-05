using CG.DAL;
using CG.Repositories.Implementations;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IApplicantProfileRepository, ApplicantProfileRepository>();
builder.Services.AddScoped<IApplicantSkillRepository, ApplicantSkillRepository>();
builder.Services.AddScoped<IApplicationStatusLogRepository, ApplicationStatusLogRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IEmployerProfileRepository, EmployerProfileRepository>();
builder.Services.AddScoped<IJobClassificationRepository, JobClassificationRepository>();
builder.Services.AddScoped<IJobPostingRepository, JobPostingRepository>();
builder.Services.AddScoped<IJobPostingSkillRepository, JobPostingSkillRepository>();
builder.Services.AddScoped<IJobSubClassificationRepository, JobSubClassificationRepository>();
builder.Services.AddScoped<IResumeRepository, ResumeRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IUserAccountRepository, UserAccountRepository>();
builder.Services.AddScoped<IWorkExperienceRepository, WorkExperienceRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();