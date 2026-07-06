namespace UniversityDataPlatform.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Admin, Gorevli
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}

