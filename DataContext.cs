using ApiRessource2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ApiRessource2
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DataContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string mySqlConnectionStr = "server = mysql-onf.alwaysdata.net; database = onf_ressources; user = onf_test; password = adminressource; Connect Timeout = 300";
            optionsBuilder.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr));
        }

    }
}
