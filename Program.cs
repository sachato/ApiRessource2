using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ApiRessource2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DataContext>(
                options => {
                    string mySqlConnectionStr = "server = mysql-onf.alwaysdata.net; database = onf_resourcedev; user = onf_test; password = adminressource; Connect Timeout = 300";
                    options.UseMySql(mySqlConnectionStr, MariaDbServerVersion.AutoDetect(mySqlConnectionStr));
        });
        var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}