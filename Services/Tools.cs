using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ApiRessource2.Services
{
    public static class Tools
    {
        public static string HashCode(string password)
        {
            //TODO: a delete ?
            var token = new JwtSecurityToken();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return passwordHash;
        }

        //TODO: a en discuter
        public static bool IsEmailValid(string address)
        {
            try
            {
                MailAddress m = new MailAddress(address);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            Regex regex = new(@"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$");
            return regex.IsMatch(phoneNumber);
        }
        public static bool IsValidPassword(string password)
        {
            Regex regex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            return regex.IsMatch(password);
        }
    }
}