using CG.DAL.Interfaces;
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
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        private readonly IUserAccountRepository _userAccountRepository;

        public DbSeeder(
            AppDbContext context,
            ILogger<DbSeeder> logger,
            IConfiguration configuration,
            IPasswordService passwordService,
            IUserAccountRepository userAccountRepository
        )
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _passwordService = passwordService;
            _userAccountRepository = userAccountRepository;
        }

        public async Task SeedAdmin()
        {
            var adminEmail = _configuration.GetValue<string>("Seed:AdminEmail");
            var adminPassword = _configuration.GetValue<string>("Seed:AdminPassword");
            var adminFirstName = _configuration.GetValue<string>("Seed:AdminFirstName");
            var adminLastName = _configuration.GetValue<string>("Seed:AdminLastName");            

            if (adminEmail is null)
                throw new InvalidOperationException("[Seeding] 'Seed:AdminEmail' is not configured.");
            if (adminPassword is null)
                throw new InvalidOperationException("[Seeding] 'Seed:AdminPassword' is not configured.");
            else if (adminFirstName is null)
                throw new InvalidOperationException("[Seeding] 'Seed:AdminFirstName' is not configured.");
            else if (adminLastName is null)
                throw new InvalidOperationException("[Seeding] 'Seed:AdminLastName' is not configured.");

            var existingAdmin = await _userAccountRepository.GetByEmailAsync(adminEmail);

            if (existingAdmin is not null)
            {
                _logger.LogInformation("[Seeding] Skipped seeding process. Admin exists already.");
                return;
            }
            
            var adminAccount = new UserAccount
            {
                FirstName = adminFirstName,
                LastName = adminLastName,
                Email = adminEmail,
                Role = Role.Admin
            };

            adminAccount.PasswordHash = _passwordService.Hash(adminAccount, adminPassword);

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