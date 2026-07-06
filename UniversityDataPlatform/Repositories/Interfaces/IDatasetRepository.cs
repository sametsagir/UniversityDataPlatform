using UniversityDataPlatform.Models;

namespace UniversityDataPlatform.Repositories.Interfaces
{
    public interface IDatasetRepository
    {
        // Temel Veri Seti İşlemleri
        Task<IEnumerable<Dataset>> GetAllWithDetailsAsync();
        Task<Dataset?> GetByIdAsync(int id);
        Task AddAsync(Dataset dataset);

        // --- ANALİZ SONUÇLARI İÇİN YENİ METOTLAR ---

        // Mevcut analizi veritabanından çekmek için (Python çalıştırmadan önce kontrol edeceğiz)
        Task<DatasetAnalysis?> GetAnalysisByDatasetIdAsync(int datasetId);

        // Yeni analiz sonucunu veritabanına kalıcı olarak kaydetmek için
        Task SaveAnalysisAsync(DatasetAnalysis analysis);

        // Akademik Atıflar & Referanslar Metotları
        Task<List<DatasetPaper>> GetPapersByDatasetIdAsync(int datasetId);
        Task AddPaperAsync(DatasetPaper paper);
        Task DeletePaperAsync(int paperId);
    }
}