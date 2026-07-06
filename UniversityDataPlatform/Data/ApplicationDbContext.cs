using Microsoft.EntityFrameworkCore;
using UniversityDataPlatform.Models;

namespace UniversityDataPlatform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Dataset> Datasets { get; set; }

        // KRİTİK EKLEME: Analiz sonuçlarını tutacak tablo
        public DbSet<DatasetAnalysis> DatasetAnalyses { get; set; }

        // Akademik Atıflar & Referanslar
        public DbSet<DatasetPaper> DatasetPapers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Dataset - User İlişkisi
            modelBuilder.Entity<Dataset>()
                .HasOne(d => d.UploaderUser)
                .WithMany()
                .HasForeignKey(d => d.UploaderUserId)
                .OnDelete(DeleteBehavior.NoAction);

            // User - Faculty İlişkisi
            modelBuilder.Entity<User>()
                .HasOne(u => u.Faculty)
                .WithMany(f => f.Users)
                .HasForeignKey(u => u.FacultyId);

            // Dataset - Faculty İlişkisi
            modelBuilder.Entity<Dataset>()
                .HasOne(d => d.Faculty)
                .WithMany(f => f.Datasets)
                .HasForeignKey(d => d.FacultyId);

            // --- YENİ: Dataset - DatasetAnalysis İlişkisi (One-to-One) ---
            modelBuilder.Entity<DatasetAnalysis>()
                .HasOne(a => a.Dataset)
                .WithOne() // Her veri setinin bir analizi olur
                .HasForeignKey<DatasetAnalysis>(a => a.DatasetId)
                .OnDelete(DeleteBehavior.Cascade);

            // DatasetPaper - Dataset İlişkisi
            modelBuilder.Entity<DatasetPaper>()
                .HasOne(p => p.Dataset)
                .WithMany()
                .HasForeignKey(p => p.DatasetId)
                .OnDelete(DeleteBehavior.Cascade);

            // Faculty Seed Data
            modelBuilder.Entity<Faculty>().HasData(
                new Faculty { Id = 1, Name = "Faculty of Engineering" },
                new Faculty { Id = 2, Name = "Faculty of Medicine" },
                new Faculty { Id = 3, Name = "Safranbolu Faculty of Architecture" },
                new Faculty { Id = 4, Name = "Faculty of Theology" },
                new Faculty { Id = 5, Name = "Faculty of Letters" },
                new Faculty { Id = 6, Name = "Faculty of Science" },
                new Faculty { Id = 7, Name = "Faculty of Business Administration" },
                new Faculty { Id = 8, Name = "Faculty of Forestry" },
                new Faculty { Id = 9, Name = "Safranbolu Faculty of Tourism" },
                new Faculty { Id = 10, Name = "Safranbolu Faculty of Fine Arts and Design" },
                new Faculty { Id = 11, Name = "Safranbolu Faculty of Communication" },
                new Faculty { Id = 12, Name = "Faculty of Health Sciences" },
                new Faculty { Id = 13, Name = "Faculty of Technology" },
                new Faculty { Id = 14, Name = "Hasan Doğan Faculty of Sports Sciences" }
            );

            // Admin User Seed Data
            var adminUser = new User
            {
                Id = 1,
                FullName = "Samet Sağır",
                Email = "samet@karabuk.edu.tr",
                Password = "AQAAAAIAAYagAAAAEInhFmTF3UaRUXnRJaTYjpu9BgJaha6efUVp1FYfqzIv+DFzFkXfDJJquhh85675Zg==", // Hashed password for "123"
                Role = "Admin",
                FacultyId = 1
            };

            modelBuilder.Entity<User>().HasData(adminUser);
        }
    }
}