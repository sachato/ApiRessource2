
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiRessource2.Models;
using ApiRessource2.Helpers;
using Microsoft.AspNetCore.Identity;

namespace ApiRessource2.Services
{

    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model, User user);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
    {
        new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
    };

        private readonly AppSettings _appSettings;
        private readonly DataContext _context;


        public UserService(IOptions<AppSettings> appSettings, DataContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

    public AuthenticateResponse Authenticate(AuthenticateRequest model, User user)
    {
        // return null if user not found
        if (user == null) return null;
        //var userverif = _users.SingleOrDefault(x => x.Email == model.Email);
        if (user == null)
            return null;
        if(model.Email == user.Email && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
        {
            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

                return new AuthenticateResponse(user, token);
            }
            else
            {
                return null;
            }


        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}