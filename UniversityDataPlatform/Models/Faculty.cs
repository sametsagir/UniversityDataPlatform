namespace UniversityDataPlatform.Models
{
    public class Faculty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Bir fakültenin birden fazla kullanıcısı ve veri seti olabilir
        public ICollection<User> Users { get; set; }
        public ICollection<Dataset> Datasets { get; set; } // ICollection kullanma sebebimiz birden fazla kullanıcı dataset yükleyebilir
    }


}
