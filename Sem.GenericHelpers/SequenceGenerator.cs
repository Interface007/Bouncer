namespace Sem.GenericHelpers
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides access to sequences of elements
    /// </summary>
    public static class SequenceGenerator
    {
        /// <summary>
        /// A sequence of all bytes
        /// </summary>
        /// <returns> An IEnumerable of bytes. </returns>
        public static IEnumerable<byte> Bytes()
        {
            for (byte i = 0; i < 255; i++)
            {
                yield return i;
            }

            yield return 255;
        }

        /// <summary>
        /// A sequence of all bytes
        /// </summary>
        /// <returns> An IEnumerable of bytes. </returns>
        public static IEnumerable<int> Ints()
        {
            var i = 0;
            while (i < int.MaxValue)
            {
                yield return i++;
            }

            yield return int.MaxValue;
        }
    }
}