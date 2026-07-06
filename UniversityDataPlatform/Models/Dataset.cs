    using UniversityDataPlatform.Models;

    public class Dataset
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public long FileSize { get; set; } // Byte cinsinden
        public string Description { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public int FacultyId { get; set; }
        public int UploaderUserId { get; set; }

        public string SubjectArea { get; set; } = "Awaiting Analysis"; // Subject area
        public string FeatureType { get; set; } = "CSV/Excel";        // Feature type
        public int InstancesCount { get; set; } = 0;                  // Row count
        public string Keywords { get; set; } = "University, Data";    // Keywords

        public Faculty? Faculty { get; set; }
        public int? FeaturesCount { get; set; } // Sütun sayısını saklamak için
        public User? UploaderUser { get; set; }
    }