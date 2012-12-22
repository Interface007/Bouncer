using System.Collections.Generic;
using System.Linq;

namespace Sem.GenericHelpers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class Extensions
    {
        /// <summary>
        /// Holds an array with the hexadecimal representation of all upper bound 4 bits of a values a byte can take
        /// </summary>
        private static readonly char[] UpperBound4BitsChar = SequenceGenerator.Bytes().Select(i => i < 16 ? '0' : Convert.ToString(i, 16)[0]).ToArray();

        /// <summary>
        /// Holds an array with the hexadecimal representation of all lower bound 4 bits of a values a byte can take
        /// </summary>
        private static readonly char[] LowerBound4BitsChar = SequenceGenerator.Bytes().Select(i => Convert.ToString(i, 16)[i < 16 ? 0 : 1]).ToArray();

        public static T[] ToArrayEx<T>(this IEnumerable<T> source)
        {
            return source as T[] ?? (source != null ? source.ToArray() : null);
        }

        /// <summary>
        /// Generates an SHA1-Hash of the data.
        /// </summary>
        /// <param name="text"> The data to create a hash for. </param>
        /// <returns> The hex-encoded hash. </returns>
        public static string GetSHA1Hash(this string text)
        {
            byte[] arrayResult;

            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                var arrayData = Encoding.ASCII.GetBytes(text);
                arrayResult = sha1.ComputeHash(arrayData);
            }

            var length = arrayResult.Length;
            var sb = new StringBuilder(length * 2);
            for (var index = 0; index < length; index++)
            {
                var t = arrayResult[index];
                sb.Append(UpperBound4BitsChar[t]);
                sb.Append(LowerBound4BitsChar[t]);
            }

            return sb.ToString();
        }
    }
}
