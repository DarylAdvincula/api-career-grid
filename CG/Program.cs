using System.Text.Json.Serialization;
using CG.DAL;
using CG.DAL.Implementations;
using CG.DAL.Interfaces;
using CG.Exceptions;
using CG.Helpers.Implementations;
using CG.Helpers.Interfaces;
using CG.Repositories.Implementations;
using CG.Repositories.Interfaces;
using CG.Services.Implementations;
using CG.Services.Interfaces;
using CG.Validators.Applicant;
using CG.Validators.ApplicantSkill;
using CG.Validators.Company;
using CG.Validators.Skill;
using CG.Validators.UserAccount;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPasswordService, PasswordService>();
builder.Services.AddSingleton<ISystemConfiguration, SystemConfiguration>();

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var systemConfig = serviceProvider
        .GetRequiredService<ISystemConfiguration>();

    options.UseSqlServer(systemConfig.DatabaseConnectionString);
});

builder.Services.AddScoped<IApplicantProfileRepository, ApplicantProfileRepository>();
builder.Services.AddScoped<IApplicantSkillRepository, ApplicantSkillRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
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
builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IApplicantProfileService, ApplicantProfileService>();
builder.Services.AddScoped<IApplicantSkillService, ApplicantSkillService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IApplicationStatusLogService, ApplicationStatusLogService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IEmployerProfileService, EmployerProfileService>();
builder.Services.AddScoped<IJobClassificationService, JobClassificationService>();
builder.Services.AddScoped<IJobPostingService, JobPostingService>();
builder.Services.AddScoped<IJobPostingSkillService, JobPostingSkillService>();
builder.Services.AddScoped<IJobSubClassificationService, JobSubClassificationService>();
builder.Services.AddScoped<IResumeService, ResumeService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IWorkExperienceService, WorkExperienceService>();
builder.Services.AddScoped<IUserSessionService, UserSessionService>();

builder.Services.AddOptions();

builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddTransient<IDbSeeder, DbSeeder>();

builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services
    .AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
    .Configure<ISystemConfiguration>((options, systemConfig) =>
    {
        options.Cookie.Name = "cg_auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromHours(systemConfig.LifeSpanSession);
        options.Events.OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<UserAccountCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CompanyCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CompanyJoinValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SkillCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ApplicantSkillCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ApplicantProfileCreateValidator>();

builder.Services.AddOptions<CorsOptions>()
    .Configure<ISystemConfiguration>((options, systemConfig) =>
    {
        options.AddPolicy("FrontendPolicy", policy =>
        {
            policy
                .WithOrigins(systemConfig.AppFrontendUrl)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<IDbSeeder>()
        .SeedAsync();
}

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ISystemConfiguration>()
        .CheckCompleteness();
}

app.Run();