using FinanceTracker.Api.Data;
using FinanceTracker.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByEmailAsync(string email) =>
        _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public Task<User?> GetByIdAsync(Guid id) =>
        _context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public async Task AddAsync(User user) =>
        await _context.Users.AddAsync(user);

    public Task<RefreshToken?> GetRefreshTokenAsync(string token) =>
        _context.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.Token == token);

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken) =>
        await _context.RefreshTokens.AddAsync(refreshToken);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
