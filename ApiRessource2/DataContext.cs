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
                modelBuilder.Entity<User>().HasData(new User { Id = 1, LastName = "Moreau", FirstName = "Kévin", IsConfirmed = true, IsDeleted = false, Role = Role.User, CreationDate = DateTime.Now, Email = "kevin.moreau2@viacesi.fr", Password = BCrypt.Net.BCrypt.HashPassword("Test123456789!"), PhoneNumber = "+33672920837", Username = "Keke", ZoneGeoId = 1 });
                modelBuilder.Entity<User>().HasData(new User { Id = 2, LastName = "Tortelli", FirstName = "Sacha", IsConfirmed = true, IsDeleted = false, Role = Role.User, CreationDate = DateTime.Now, Email = "sacha.tortelli@viacesi.fr", Password = BCrypt.Net.BCrypt.HashPassword("test"), PhoneNumber = "+33672920835", Username = "Sachou", ZoneGeoId = 1 });
                
                modelBuilder.Entity<User>().HasData(new User { Id = 3, LastName = "User", FirstName = "User", IsConfirmed = true, IsDeleted = false, Role = Role.User, CreationDate = DateTime.Now, Email = "User@viacesi.fr", Password = BCrypt.Net.BCrypt.HashPassword("Test123456789!"), PhoneNumber = "+33672920835", Username = "User", ZoneGeoId = 1 });
                modelBuilder.Entity<User>().HasData(new User { Id = 4, LastName = "NotOwner", FirstName = "NotOwner", IsConfirmed = true, IsDeleted = false, Role = Role.User, CreationDate = DateTime.Now, Email = "NotOwner@viacesi.fr", Password = BCrypt.Net.BCrypt.HashPassword("Test123456789!"), PhoneNumber = "+33672920835", Username = "NotOwner", ZoneGeoId = 1 });
                modelBuilder.Entity<User>().HasData(new User { Id = 5, LastName = "Admin", FirstName = "Admin", IsConfirmed = true, IsDeleted = false, Role = Role.Administrator, CreationDate = DateTime.Now, Email = "Admin@viacesi.fr", Password = BCrypt.Net.BCrypt.HashPassword("Test123456789!"), PhoneNumber = "+33672920835", Username = "Admin", ZoneGeoId = 1 });
                modelBuilder.Entity<User>().HasData(new User { Id = 6, LastName = "Moderateur", FirstName = "Moderateur", IsConfirmed = true, IsDeleted = false, Role = Role.Moderator, CreationDate = DateTime.Now, Email = "Moderateur@viacesi.fr", Password = BCrypt.Net.BCrypt.HashPassword("Test123456789!"), PhoneNumber = "+33672920835", Username = "Moderateur", ZoneGeoId = 1 });
                modelBuilder.Entity<User>().HasData(new User { Id = 7, LastName = "SuperAdmin", FirstName = "SuperAdmin", IsConfirmed = true, IsDeleted = false, Role = Role.SuperAdministrator, CreationDate = DateTime.Now, Email = "SuperAdmin@viacesi.fr", Password = BCrypt.Net.BCrypt.HashPassword("Test123456789!"), PhoneNumber = "+33672920835", Username = "SuperAdmin", ZoneGeoId = 1 });

                modelBuilder.Entity<Resource>().HasData(new Resource { Id = 1, CreationDate = DateTime.Now, Description = "Description1", DownVote = 0, UpVote = 0, IsDeleted = false, Path = "", Title = "Ressource1", Type = TypeRessource.Texte, UserId = 1 });
                modelBuilder.Entity<Resource>().HasData(new Resource { Id = 2, CreationDate = DateTime.Now, Description = "Description2", DownVote = 0, UpVote = 0, IsDeleted = false, Path = "", Title = "Ressource2", Type = TypeRessource.Document, UserId = 1 });

                modelBuilder.Entity<Comment>().HasData(new Comment { Id = 1, DatePost = DateTime.Now, Content = "Description1", IsDeleted = false, ResourceId = 1, UserId = 3 });
                modelBuilder.Entity<Comment>().HasData(new Comment { Id = 2, DatePost = DateTime.Now, Content = "Description2", IsDeleted = false, ResourceId = 1, UserId = 3 });
                modelBuilder.Entity<Comment>().HasData(new Comment { Id = 3, DatePost = DateTime.Now, Content = "Description3", IsDeleted = false, ResourceId = 1, UserId = 1 }); //NotOwner
                modelBuilder.Entity<Comment>().HasData(new Comment { Id = 4, DatePost = DateTime.Now, Content = "Description2", IsDeleted = false, ResourceId = 2, UserId = 1 });


            }
        }


        /*//Configurations des tables

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Resource//
            *//*builder.Entity<Resource>(entity =>
            {
                entity.ToTable("Resources");
                entity.HasKey(r => r.Id);
                entity.Property(r=>r.Description).IsRequired();
                entity.Property(r => r.DownVote).IsRequired();
                entity.Property(r => r.UpVote).IsRequired();
                entity.Property(r => r.Type).IsRequired();
                entity.Property(r => r.CreationDate).IsRequired();
                entity.Property(r => r.Title).IsRequired();
                entity.HasMany(r=>r.Comments).WithOne().HasForeignKey(r=>r.Id).IsRequired();
            });

            //Comment//
            builder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comments");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Content).IsRequired();
                entity.Property(c => c.UserId).HasColumnName("UserId").IsRequired();
                entity.Property(c => c.ResourceId).HasColumnName("ResourceId").IsRequired();
            });*//*
        }*/



    }
}