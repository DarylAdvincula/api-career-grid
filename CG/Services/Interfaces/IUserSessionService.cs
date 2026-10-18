using CG.DTO.UserSession;

namespace CG.Services.Interfaces
{
    public interface IUserSessionService
    {
        Task<UserSessionDto> GetByIdAsync(Guid id);

        Task<UserSessionDto> CreateAsync(UserSessionCreateDto request);

        Task RevokeAsync(Guid id);

        Task RevokeAllOtherActiveAsync(
            Guid id, 
            int userAccountId
        );

        Task DeleteAllRevokedAsync();

        Task DeleteAllExpiredAsync();
    }
}