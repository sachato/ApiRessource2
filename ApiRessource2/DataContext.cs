using ApiRessource2.Models;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;

namespace ApiRessource2
{
    public class DataContext : DbContext
    {
        private readonly IWebHostEnvironment hostEnvironment;
        public DbSet<User> Users { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Favoris> Favoris { get; set; }
        public DbSet<ReportedComment> ReportedComments { get; set; }
        public DbSet<ReportedRessource> ReportedRessources { get; set; }
        public DbSet<Voted> Voteds { get; set; }
        public DbSet<ZoneGeo> ZoneGeos { get; set; }

        public DataContext(DbContextOptions<DataContext> options, IWebHostEnvironment hostEnvironment)
            : base(options)
        {
            this.hostEnvironment = hostEnvironment;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (hostEnvironment.IsDevelopment())
            {
                modelBuilder.Entity<User>().HasData(new User { Id = 1, LastName = "Moreau", FirstName = "Kévin", IsConfirmed = true, IsDeleted = false, Role = Role.Administrator, CreationDate = DateTime.Now, Email = "kevin.moreau2@viacesi.fr", Password = BCrypt.Net.BCrypt.HashPassword("test"), PhoneNumber = "+33672920837", Username = "Keke", ZoneGeoId = 1 });
                modelBuilder.Entity<User>().HasData(new User { Id = 2, LastName = "Tortelli", FirstName = "Sacha", IsConfirmed = true, IsDeleted = false, Role = Role.User, CreationDate = DateTime.Now, Email = "sacha.tortelli@viacesi.fr", Password = BCrypt.Net.BCrypt.HashPassword("test"), PhoneNumber = "+33672920835", Username = "Sachou", ZoneGeoId = 1 });
                modelBuilder.Entity<Resource>().HasData(new Resource { Id = 1, CreationDate = DateTime.Now, Description = "Description1", DownVote = 0, UpVote = 0, IsDeleted = false, Path = "", Title = "Ressource1", Type = TypeRessource.Texte, UserId = 1 });
            }
        }
    }
}