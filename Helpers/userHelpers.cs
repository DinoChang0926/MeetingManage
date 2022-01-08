using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace MeetingManage.Helpers
{
    public class userHelpers
    {
        private readonly IConfiguration _configuration;

        public userHelpers(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string SetPassword(string Password)
        {
            return HashObject(Password);
        }
        public bool VerPassword(string password, string req_password)
        {
            return password.Equals(HashObject(req_password));
        }
        private string HashObject(string obj)
        {
            byte[] salt = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("hashString"));
            string result = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: obj,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return result;
        }
    }
}
