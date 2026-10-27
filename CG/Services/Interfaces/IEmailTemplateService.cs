namespace CG.Services.Interfaces
{
    public interface IEmailTemplateService
    {
        Task<string> RenderAsync(string templateName, IReadOnlyDictionary<string, string> values);
    }
}