using Microsoft.EntityFrameworkCore;
using UniversityDataPlatform.Data;
using UniversityDataPlatform.Models;
using UniversityDataPlatform.Repositories.Interfaces;

namespace UniversityDataPlatform.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) => _context = context;

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.Include(u => u.Faculty).FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetByIdAsync(int id) =>
            await _context.Users.FindAsync(id);
    }
}