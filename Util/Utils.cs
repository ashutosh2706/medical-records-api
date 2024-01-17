using System.Security.Cryptography;
using System.Text;

namespace MedicalRecords.Util
{
    public static class Utils
    {
        public static string MRN()
        {
            Guid guid = Guid.NewGuid();
            string str = guid.ToString();
            return str.Substring(0, Math.Min(5, str.Length));
        }

        public static string UID()
        {
            Guid guid = Guid.NewGuid();
            string str = guid.ToString();
            return str.Substring(0, Math.Min(8, str.Length));
        }

        public static string ContentType(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName)?.ToLower();
            switch (fileExtension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".jpeg":
                case ".jpg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                default:
                    return "application/octet-stream"; // Default to binary/octet-stream for unknown types
            }
        }

        public static string Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
