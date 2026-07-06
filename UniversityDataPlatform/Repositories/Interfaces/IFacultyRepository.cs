using UniversityDataPlatform.Models;

namespace UniversityDataPlatform.Repositories.Interfaces
{
    public interface IFacultyRepository
    {
        Task<IEnumerable<Faculty>> GetAllAsync();
    }
}