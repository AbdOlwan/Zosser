using DAL.Entities.Identity;


namespace DAL.Repositories.Interfaces.Identity
{
    public interface IRefreshTokenRepo
    {
        Task<RefreshToken?> GetRefreshTokenAsync(string token, string userId);
        Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token);
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
    }
}
