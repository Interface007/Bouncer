// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamName.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the StreamName type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.IO.Ads
{
    using System;
    using System.Runtime.InteropServices;

    internal sealed class StreamName : IDisposable
    {
        private static readonly SafeHGlobalHandle InvalidBlock = SafeHGlobalHandle.Invalid();
        
        private SafeHGlobalHandle memoryBlock = InvalidBlock;

        /// <summary>
        /// Gets the handle to the block of memory.
        /// </summary>
        public SafeHGlobalHandle MemoryBlock
        {
            get { return this.memoryBlock; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.memoryBlock.IsInvalid)
            {
                return;
            }

            this.memoryBlock.Dispose();
            this.memoryBlock = InvalidBlock;
        }

        /// <summary>
        /// Ensures that there is sufficient memory allocated.
        /// </summary>
        /// <param name="capacity">
        /// The required capacity of the block, in bytes.
        /// </param>
        /// <exception cref="OutOfMemoryException">
        /// There is insufficient memory to satisfy the request.
        /// </exception>
        public void EnsureCapacity(int capacity)
        {
            var currentSize = this.memoryBlock.IsInvalid ? 0 : this.memoryBlock.Size;
            if (capacity <= currentSize)
            {
                return;
            }

            if (0 != currentSize)
            {
                currentSize <<= 1;
            }
            
            if (capacity > currentSize)
            {
                currentSize = capacity;
            }

            if (!this.memoryBlock.IsInvalid)
            {
                this.memoryBlock.Dispose();
            }
            
            this.memoryBlock = SafeHGlobalHandle.Allocate(currentSize);
        }

        /// <summary>
        /// Reads the Unicode string from the memory block.
        /// </summary>
        /// <param name="length">
        /// The length of the string to read, in characters.
        /// </param>
        /// <returns>
        /// The string read from the memory block.
        /// </returns>
        public string ReadString(int length)
        {
            if (0 >= length || this.memoryBlock.IsInvalid)
            {
                return null;
            }
            
            if (length > this.memoryBlock.Size)
            {
                length = this.memoryBlock.Size;
            }
            
            return Marshal.PtrToStringUni(this.memoryBlock.DangerousGetHandle(), length);
        }

        /// <summary>
        /// Reads the string, and extracts the stream name.
        /// </summary>
        /// <param name="length">
        /// The length of the string to read, in characters.
        /// </param>
        /// <returns>
        /// The stream name.
        /// </returns>
        public string ReadStreamName(int length)
        {
            var name = this.ReadString(length);
            if (!string.IsNullOrEmpty(name))
            {
                // Name is of the format ":NAME:$DATA\0"
                var separatorIndex = name.IndexOf(SafeNativeMethods.StreamSeparator, 1);
                if (-1 != separatorIndex)
                {
                    name = name.Substring(1, separatorIndex - 1);
                }
                else
                {
                    // Should never happen!
                    separatorIndex = name.IndexOf('\0');
                    name = 1 < separatorIndex ? name.Substring(1, separatorIndex - 1) : null;
                }
            }

            return name;
        }
    }
}
