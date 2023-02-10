using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiRessource2.Services
{
    public static class Tools
    {

        public static string HashCode(string password)
        {
            var token = new JwtSecurityToken();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return passwordHash;

        }
    }
}
