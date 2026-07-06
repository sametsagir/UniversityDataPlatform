using Microsoft.EntityFrameworkCore;
using UniversityDataPlatform.Data;
using UniversityDataPlatform.Models;
using UniversityDataPlatform.Repositories.Interfaces;

namespace UniversityDataPlatform.Repositories
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly ApplicationDbContext _context;
        public FacultyRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Faculty>> GetAllAsync()
        {
            return await _context.Faculties.ToListAsync();
        }
    }
}