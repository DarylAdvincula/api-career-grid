using System.Security.Claims;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class BaseService : IBaseService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserAccountRepository _userAccountRepository;
        
        public BaseService(
            IHttpContextAccessor httpContextAccessor,
            IUserAccountRepository userAccountRepository
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _userAccountRepository = userAccountRepository;
        }

        public HttpContext GetHttpContext()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            
            if (httpContext is null)
                throw new InvalidOperationException("No active HTTP context.");
            
            return httpContext;
        }

        public async Task<UserAccount> GetCurrentUser()
        {
            var httpContext = GetHttpContext();
            
            var rawUserAccountId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(rawUserAccountId, out var userAccountId))
                throw new UnauthorizedException("Not authenticated.");
            
            var userAccount = await _userAccountRepository.GetByIdAsync(userAccountId);

            if (userAccount is null)
                throw new UnauthorizedException("Not authenticated.");
            
            return userAccount;
        }
    }
}