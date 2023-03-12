using ApiRessource2.Controllers;
using ApiRessource2.Models;
using ApiRessource2.Test.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ApiRessource2.Test.ControllersTest
{
    public class FavorisControllerTests : IDisposable
    {
        private readonly DataContext _context;
        private readonly FavorisController _controller;

        public FavorisControllerTests()
        {
            var contentRootPath = "C:\\Users\\benja\\Documents\\GitHub\\ApiRessource2";
            var environment = new TestWebHostEnvironment(contentRootPath);

            var services = new ServiceCollection();
            services.AddSingleton<IWebHostEnvironment>(environment);
            services.AddDbContext<DataContext>(options =>
                options.UseInMemoryDatabase("onf_resourcedev2test"));


            var serviceProvider = services.BuildServiceProvider();

            _context = serviceProvider.GetService<DataContext>();
            _controller = new FavorisController(_context);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetFavorisByIdTest()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _controller.GetFavorisById(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Favoris>>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetAllFavorisByIdUserTest()
        {
            // Arrange
            User user = new User
            {
                Id = 3,
                FirstName = "test",
                LastName = "test",
                PhoneNumber = "0626252525",
                Username = "testuser",
                Email = "testuser@example.com",
                Password = "password",
                Role = Role.User,
                ZoneGeoId = 1
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
            {
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Role, user.Role.ToString()),
    new Claim("zone_geo_id", user.ZoneGeoId.ToString())
            };

            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = principal };

            // Act
            var result = await _controller.GetAllFavorisByIdUser(user.Id);

            // Assert
            Assert.NotNull(result);
            // Ajoutez d'autres assertions en fonction de la logique de votre application
        }



    }
}
