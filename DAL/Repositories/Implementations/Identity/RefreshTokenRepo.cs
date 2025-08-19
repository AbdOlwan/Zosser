using DAL.Context;
using DAL.Entities.Identity;
using DAL.Repositories.Interfaces.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementations.Identity
{
    public class RefreshTokenRepo : IRefreshTokenRepo
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token, string userId)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token && rt.UserId == userId);
        }

        public async Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rt =>
                    rt.Token == token &&
                    !rt.IsRevoked &&
                    rt.ExpiryDate > DateTime.UtcNow);
        }

        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            await Task.CompletedTask;
        }
    }
}
