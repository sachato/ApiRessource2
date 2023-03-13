using ApiRessource2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ApiRessource2
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Favoris> Favoris { get; set; }
        public DbSet<ReportedComment> ReportedComments { get; set; }
        public DbSet<ReportedRessource> ReportedRessources { get; set; }
        public DbSet<Voted> Voteds { get; set; }
        public DbSet<ZoneGeo> ZoneGeos { get; set; }

        public DataContext()
        {

        }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }


        //Configurations des tables

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Resource//
            /*builder.Entity<Resource>(entity =>
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
            });*/
        }



    }
}
