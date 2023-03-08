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
    }
}
