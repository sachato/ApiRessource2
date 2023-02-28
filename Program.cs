using ApiRessource2.Helpers;
using ApiRessource2.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Configuration;
using ApiRessource2.Helpers;
using System.Text.Json.Serialization;

namespace ApiRessource2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
            builder.Services.AddScoped<IUserService, UserService>();
            
            ConfigurationHelper.Initialize(builder.Configuration);
            builder.Services.AddDbContext<DataContext>(
                options => {
                    string mySqlConnectionStr = builder.Configuration.GetConnectionString("connectionString");
                    options.UseMySql(mySqlConnectionStr, MariaDbServerVersion.AutoDetect(mySqlConnectionStr));
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
            
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<JwtMiddleware>();


            app.MapControllers();

            app.Run();
        }
    }
}