using Microsoft.EntityFrameworkCore;
using UniversityDataPlatform.Data;
using UniversityDataPlatform.Models;
using UniversityDataPlatform.Repositories.Interfaces;

namespace UniversityDataPlatform.Repositories
{
    public class DatasetRepository : IDatasetRepository
    {
        private readonly ApplicationDbContext _context;

        public DatasetRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Dataset>> GetAllWithDetailsAsync()
        {
            return await _context.Datasets
                .Include(d => d.Faculty)
                .Include(d => d.UploaderUser)
                .OrderByDescending(d => d.UploadDate)
                .ToListAsync();
        }

        public async Task<Dataset?> GetByIdAsync(int id)
        {
            return await _context.Datasets
                .Include(d => d.Faculty)
                .Include(d => d.UploaderUser)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddAsync(Dataset dataset)
        {
            await _context.Datasets.AddAsync(dataset);
            await _context.SaveChangesAsync();
        }

        // --- HATA ÇÖZÜMÜ: YENİ METOTLARIN İMPLEMENTASYONU ---

        public async Task<DatasetAnalysis?> GetAnalysisByDatasetIdAsync(int datasetId)
        {
            // Veritabanında bu veri setine ait daha önce yapılmış bir analiz var mı bakıyoruz
            return await _context.DatasetAnalyses
                .FirstOrDefaultAsync(a => a.DatasetId == datasetId);
        }

        public async Task SaveAnalysisAsync(DatasetAnalysis analysis)
        {
            // 1. Analiz detayını kaydet
            await _context.DatasetAnalyses.AddAsync(analysis);

            // 2. ANA DATASET TABLOSUNU GÜNCELLE (Bu kısım eksik olduğu için filtre çalışmıyor)
            var dataset = await _context.Datasets.FindAsync(analysis.DatasetId);
            if (dataset != null)
            {
                // Python'dan gelen "Classification" veya "Regression" bilgisini ana tabloya işle
                dataset.SubjectArea = analysis.TaskType;
                dataset.InstancesCount = analysis.Instances;
                dataset.FeaturesCount = analysis.Features;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<DatasetPaper>> GetPapersByDatasetIdAsync(int datasetId)
        {
            return await _context.DatasetPapers
                .Where(p => p.DatasetId == datasetId)
                .ToListAsync();
        }

        public async Task AddPaperAsync(DatasetPaper paper)
        {
            try
            {
                await _context.DatasetPapers.AddAsync(paper);
                await _context.SaveChangesAsync();
            }
            catch
            {
                _context.ChangeTracker.Clear();
                throw;
            }
        }

        public async Task DeletePaperAsync(int paperId)
        {
            try
            {
                var paper = await _context.DatasetPapers.FindAsync(paperId);
                if (paper != null)
                {
                    _context.DatasetPapers.Remove(paper);
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                _context.ChangeTracker.Clear();
                throw;
            }
        }
    }
}