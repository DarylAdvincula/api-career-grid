using CG.DAL.Interfaces;
using CG.Helpers.Interfaces;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.DAL.Implementations
{
    public class DbSeeder : IDbSeeder
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DbSeeder> _logger;
        private readonly ISystemConfiguration _systemConfiguration;
        private readonly IPasswordService _passwordService;
        private readonly IUserAccountRepository _userAccountRepository;

        public DbSeeder(
            AppDbContext context,
            ILogger<DbSeeder> logger,
            ISystemConfiguration systemConfiguration,
            IPasswordService passwordService,
            IUserAccountRepository userAccountRepository
        )
        {
            _context = context;
            _logger = logger;
            _systemConfiguration = systemConfiguration;
            _passwordService = passwordService;
            _userAccountRepository = userAccountRepository;
        }

        public async Task SeedAdmin()
        {
            var existingAdmin = await _userAccountRepository.GetByEmailAsync(_systemConfiguration.AdminEmail);

            if (existingAdmin is not null)
            {
                _logger.LogInformation("[Seeding] Skipped seeding process. Admin exists already.");
                return;
            }
            
            var adminAccount = new UserAccount
            {
                FirstName = _systemConfiguration.AdminFirstName,
                LastName = _systemConfiguration.AdminLastName,
                Email = _systemConfiguration.AdminEmail,
                IsVerified = true,
                Role = Role.Admin
            };

            adminAccount.PasswordHash = _passwordService.Hash(adminAccount, _systemConfiguration.AdminPassword);

            await _userAccountRepository.AddAsync(adminAccount);
            await _userAccountRepository.SaveChangesAsync();
            _logger.LogInformation("[Seeding] Admin has been seeded.");
        }
        
        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();
            _logger.LogInformation("[Seeding] Migrations ran.");
            await SeedAdmin();
        }
    }
}