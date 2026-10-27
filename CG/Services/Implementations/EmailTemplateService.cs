using System.Net;
using System.Text.RegularExpressions;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public sealed class EmailTemplateService : IEmailTemplateService
    {
        IWebHostEnvironment _environment;

        public EmailTemplateService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        
        public async Task<string> RenderAsync(string templateName, IReadOnlyDictionary<string, string> values)
        {
            if (!Regex.IsMatch(templateName, @"\A[A-Za-z0-9_-]+\z"))
                throw new ArgumentException("Invalid template name.", nameof(templateName));

            var path = Path.Combine(_environment.ContentRootPath, "Templates", "Emails", $"{templateName}.html");
            var html = await File.ReadAllTextAsync(path);

            foreach (var (key, value) in values)
            {
                var placeholder = $"{{{{{key}}}}}";
                html = html.Replace(placeholder, WebUtility.HtmlEncode(value), StringComparison.Ordinal);
            }

            return html;
        }
    }
}