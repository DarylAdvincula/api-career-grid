using CG.DTO.UserSession;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class UserSessionService : IUserSessionService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserSessionRepository _userSessionRepository;

        public UserSessionService(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IUserSessionRepository userSessionRepository
        )
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userSessionRepository = userSessionRepository;
        }

        private async Task<UserSession> GetByIdOrThrowAsync(Guid id)
        {
            var userSession = await _userSessionRepository.GetByIdAsync(id);

            if (userSession is null)
                throw new UnauthorizedException($"Invalid session.");
            
            return userSession;
        }

        private UserSessionDto ToDto(UserSession userSession)
        {
            return new UserSessionDto
            {
                Id = userSession.Id,
                UserAccountId = userSession.UserAccountId,
                IpAddress = userSession.IpAddress,
                UserAgent = userSession.UserAgent,
                RevokedAt = userSession.RevokedAt,
                ExpiresAt = userSession.ExpiresAt,
                CreatedAt = userSession.CreatedAt,
            };
        }

        public async Task<UserSessionDto> GetByIdAsync(Guid id)
        {
            var userSession = await GetByIdOrThrowAsync(id);
            return ToDto(userSession);
        }

        public async Task<UserSessionDto> CreateAsync(UserSessionCreateDto request)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var ipAddress = httpContext?.Connection.RemoteIpAddress?.ToString();
            var userAgent = httpContext?.Request.Headers.UserAgent.ToString();

            var validityDurationHours = _configuration.GetValue<int>("AppSettings:SessionValidityDurationHours");
            var expiresAt = DateTime.Now.AddHours(validityDurationHours);
            
            var newUserSession = new UserSession
            {
                Id = request.Id,
                UserAccountId = request.UserAccountId,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                ExpiresAt = expiresAt
            };

            await _userSessionRepository.AddAsync(newUserSession);
            await _userSessionRepository.SaveChangesAsync();
            return ToDto(newUserSession);
        }

        public async Task RevokeAsync(Guid id)
        {
            var userSession = await GetByIdOrThrowAsync(id);
            userSession.RevokedAt = DateTime.Now;

            _userSessionRepository.Update(userSession);
            await _userSessionRepository.SaveChangesAsync();
        }

        public async Task RevokeAllOtherActiveAsync(
            Guid id,
            int userAccountId
        )
        {   
            var otherActiveUserSessions = await _userSessionRepository.GetAllOtherActiveByUserAccountIdAsync(
                id, 
                userAccountId, 
                true
            );
            
            foreach (var otherActiveUserSession in otherActiveUserSessions)
            {
                _userSessionRepository.Delete(otherActiveUserSession);
            }

            await _userSessionRepository.SaveChangesAsync();
        }

        public async Task DeleteAllRevokedAsync()
        {
            var revokedUserSessions = await _userSessionRepository.GetAllRevokedAsync(true);

            foreach (var revokedUserSession in revokedUserSessions)
            {
                _userSessionRepository.Delete(revokedUserSession);
            }

            await _userSessionRepository.SaveChangesAsync();
        }

        public async Task DeleteAllExpiredAsync()
        {
            var expiredUserSessions = await _userSessionRepository.GetAllExpiredAsync(true);

            foreach (var expiredUserSession in expiredUserSessions)
            {
                _userSessionRepository.Delete(expiredUserSession);
            }

            await _userSessionRepository.SaveChangesAsync();
        }
    }
}