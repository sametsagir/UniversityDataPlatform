using System.ComponentModel.DataAnnotations;

namespace UniversityDataPlatform.Models
{
    public class DatasetPaper
    {
        public int Id { get; set; }
        public int DatasetId { get; set; }
        
        [Required(ErrorMessage = "Kaynak başlığı zorunludur.")]
        public string Title { get; set; } = string.Empty;
        
        public string Authors { get; set; } = string.Empty;
        
        public int Year { get; set; }
        
        public string Venue { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Kaynak bağlantısı (URL) zorunludur.")]
        [Url(ErrorMessage = "Lütfen geçerli bir internet adresi (URL) girin (Örn: https://example.com).")]
        public string Url { get; set; } = string.Empty;

        // Navigation property
        public virtual Dataset? Dataset { get; set; }
    }
}
