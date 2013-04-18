// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SafeHGlobalHandle.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   A <see cref="SafeHandle" /> for a global memory allocation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.IO.Ads
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;

    /// <summary>
    /// A <see cref="SafeHandle"/> for a global memory allocation.
    /// </summary>
    internal sealed class SafeHGlobalHandle : SafeHandle
    {
        /// <summary>
        /// The size of the object.
        /// </summary>
        private readonly int size;

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeHGlobalHandle"/> class.
        /// </summary>
        /// <param name="toManage">
        /// The initial handle value.
        /// </param>
        /// <param name="size">
        /// The size of this memory block, in bytes.
        /// </param>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        private SafeHGlobalHandle(IntPtr toManage, int size)
            : base(IntPtr.Zero, true)
        {
            this.size = size;
            this.SetHandle(toManage);
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="SafeHGlobalHandle"/> class from being created. 
        /// </summary>
        private SafeHGlobalHandle()
            : base(IntPtr.Zero, true)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the handle value is invalid.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the handle value is invalid;
        /// otherwise, <see langword="false"/>.
        /// </value>
        public override bool IsInvalid
        {
            get { return IntPtr.Zero == this.handle; }
        }

        /// <summary>
        /// Gets the size of this memory block.
        /// </summary>
        public int Size
        {
            get { return this.size; }
        }

        /// <summary>
        /// Allocates memory from the unmanaged memory of the process using <c>GlobalAlloc</c>.
        /// </summary>
        /// <param name="bytes">
        /// The number of bytes in memory required.
        /// </param>
        /// <returns>
        /// A <see cref="SafeHGlobalHandle"/> representing the memory.
        /// </returns>
        /// <exception cref="OutOfMemoryException">
        /// There is insufficient memory to satisfy the request.
        /// </exception>
        public static SafeHGlobalHandle Allocate(int bytes)
        {
            return new SafeHGlobalHandle(Marshal.AllocHGlobal(bytes), bytes);
        }

        /// <summary>
        /// Returns an invalid handle.
        /// </summary>
        /// <returns>
        /// An invalid <see cref="SafeHGlobalHandle"/>.
        /// </returns>
        public static SafeHGlobalHandle Invalid()
        {
            return new SafeHGlobalHandle();
        }

        /// <summary>
        /// Executes the code required to free the handle.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the handle is released successfully;
        /// otherwise, in the event of a catastrophic failure, <see langword="false"/>.
        /// In this case, it generates a releaseHandleFailed MDA Managed Debugging Assistant.
        /// </returns>
        protected override bool ReleaseHandle()
        {
            Marshal.FreeHGlobal(this.handle);
            return true;
        }
    }
}
