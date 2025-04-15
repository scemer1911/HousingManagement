using System.Security.Cryptography;
using System.Text;

namespace HousingManagement.Helpers
{
    public static class PasswordHelper
    {
        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                foreach (var b in hashBytes)
                    builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }
    }
}
