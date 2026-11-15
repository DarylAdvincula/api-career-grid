using CG.DAL.Interfaces;
using CG.Helpers.Interfaces;

namespace CG.Helpers.Implementations
{
    public class SystemSetup : ISystemSetup
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISystemConfiguration _systemConfiguration;
        private readonly IDbSeeder _dbSeeder;
        
        public SystemSetup(
            IWebHostEnvironment webHostEnvironment,
            ISystemConfiguration systemConfiguration,
            IDbSeeder dbSeeder
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _systemConfiguration = systemConfiguration;
            _dbSeeder = dbSeeder;
        }

        private void MakeDirectoriesSync()
        {
            var contentRootPath = _webHostEnvironment.ContentRootPath;
            var directories = new List<string>
            {
                Path.Combine(contentRootPath, _systemConfiguration.UploadsResumes),
            };

            foreach (var directory in directories)
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }
        }

        public async Task RunAsync()
        {
            _systemConfiguration.CheckCompleteness();
            MakeDirectoriesSync();
            await _dbSeeder.SeedAsync();
        }
    }
}