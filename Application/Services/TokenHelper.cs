using System;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public static class TokenHelper
    {
        public static string GenerateTokenRaw(int size = 32)
        {
            var data = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);
            // base64url encode
            return Base64UrlEncode(data);
        }

        public static string HashToken(string token)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(token);
            var hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
        }

        private static string Base64UrlEncode(byte[] input)
        {
            var base64 = Convert.ToBase64String(input);
            return base64.Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
    }
}
