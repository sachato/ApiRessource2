using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;

namespace ApiRessource2.Services
{
    public static class Tools
    {

        public static string HashCode(string password)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password, "lol");
            return passwordHash;
        }
    }
}
