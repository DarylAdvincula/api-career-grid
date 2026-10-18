using CG.Models;

namespace CG.Services.Interfaces
{
    public interface IBaseService
    {
        public HttpContext GetHttpContext();

        Task<UserAccount> GetCurrentUser();
    }
}