using System.IdentityModel.Tokens.Jwt;

namespace ApiRessource2.Models;


public class AuthenticateResponse
{
    public int id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public Role Role { get; set; }
    public string Token { get; set; }


    public AuthenticateResponse(User user, string token)
    {
        id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Username = user.Username;
        Role = user.Role;
        Token = token;
    }

    static public int GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        return int.Parse(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "id")?.Value);
    }

}