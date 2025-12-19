using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Helpers
{
    public static class TripCodeGenerator
    {
        private const string Alphabet = "ABCDEFGHJKMNPQRSTUVWXYZ23456789"; // من غير 0,1,O,I

        public static string Generate(int length = 4)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            Span<byte> bytes = stackalloc byte[length];
            RandomNumberGenerator.Fill(bytes);

            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                var index = bytes[i] % Alphabet.Length;
                sb.Append(Alphabet[index]);
            }

            return sb.ToString();
        }
    }
    
}
