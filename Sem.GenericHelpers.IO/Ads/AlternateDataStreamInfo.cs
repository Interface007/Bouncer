// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlternateDataStreamInfo.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Represents the details of an alternative data stream.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.IO.Ads
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Security;
    using System.Security.Permissions;

    /// <summary>
    /// Represents the details of an alternative data stream.
    /// </summary>
    [DebuggerDisplay("{FullPath}")]
    public sealed class AlternateDataStreamInfo : IEquatable<AlternateDataStreamInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlternateDataStreamInfo"/> class.
        /// </summary>
        /// <param name="filePath"> The full path of the file. This argument must not be <see langword="null"/>. </param>
        /// <param name="info"> The <see cref="SafeNativeMethods.Win32StreamInfo"/> containing the stream information. </param>
        internal AlternateDataStreamInfo(string filePath, SafeNativeMethods.Win32StreamInfo info)
        {
            this.FilePath = filePath;
            this.Name = info.StreamName;
            this.StreamType = info.StreamType;
            this.Attributes = info.StreamAttributes;
            this.Size = info.StreamSize;
            this.Exists = true;

            this.FullPath = SafeNativeMethods.BuildStreamPath(this.FilePath, this.Name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlternateDataStreamInfo"/> class.
        /// </summary>
        /// <param name="filePath"> The full path of the file. This argument must not be <see langword="null"/>. </param>
        /// <param name="streamName"> The name of the stream This argument must not be <see langword="null"/>. </param>
        /// <param name="fullPath"> The full path of the stream. If this argument is <see langword="null"/>, it will be generated from the  <paramref name="filePath"/> and <paramref name="streamName"/> arguments. </param>
        /// <param name="exists"> <see langword="true"/> if the stream exists; otherwise, <see langword="false"/>. </param>
        internal AlternateDataStreamInfo(string filePath, string streamName, string fullPath, bool exists)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                fullPath = SafeNativeMethods.BuildStreamPath(filePath, streamName);
            }

            this.StreamType = FileStreamType.AlternateDataStream;

            this.FilePath = filePath;
            this.Name = streamName;
            this.FullPath = fullPath;
            this.Exists = exists;

            if (this.Exists)
            {
                this.Size = SafeNativeMethods.GetFileSize(this.FullPath);
            }
        }

        /// <summary>
        /// Gets the full path of this stream.
        /// </summary>
        /// <value> The full path of this stream. </value>
        public string FullPath { get; private set; }

        /// <summary>
        /// Gets the full path of the file which contains the stream.
        /// </summary>
        /// <value> The full file-system path of the file which contains the stream. </value>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets the name of the stream.
        /// </summary>
        /// <value> The name of the stream. </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the specified stream exists.
        /// </summary>
        /// <value> <see langword="true"/> if the stream exists; otherwise, <see langword="false"/>.
        /// </value>
        public bool Exists { get; private set; }

        /// <summary>
        /// Gets the size of the stream, in bytes.
        /// </summary>
        /// <value> The size of the stream, in bytes. </value>
        public long Size { get; private set; }

        /// <summary>
        /// Gets the type of data.
        /// </summary>
        /// <value> One of the <see cref="FileStreamType"/> values. </value>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public FileStreamType StreamType { get; private set; }

        /// <summary>
        /// Gets attributes of the data stream.
        /// </summary>
        /// <value> A combination of <see cref="FileStreamAttributes"/> values. </value>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public FileStreamAttributes Attributes { get; private set; }

        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="first"> The first object. </param>
        /// <param name="second"> The second object. </param>
        /// <returns> <see langword="true"/> if the two objects are equal; otherwise, <see langword="false"/>. </returns>
        public static bool operator ==(AlternateDataStreamInfo first, AlternateDataStreamInfo second)
        {
            return object.ReferenceEquals(first, second)
                   || (!object.ReferenceEquals(null, first) && (!object.ReferenceEquals(null, second) && first.Equals(second)));
        }

        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="first"> The first object. </param>
        /// <param name="second"> The second object. </param>
        /// <returns> <see langword="true"/> if the two objects are not equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(AlternateDataStreamInfo first, AlternateDataStreamInfo second)
        {
            return !object.ReferenceEquals(first, second)
                   && (object.ReferenceEquals(null, first) || (object.ReferenceEquals(null, second) || !first.Equals(second)));
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current instance.
        /// </summary>
        /// <returns> A <see cref="String"/> that represents the current instance. </returns>
        public override string ToString()
        {
            return this.FullPath;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns> A hash code for the current <see cref="Object"/>. </returns>
        public override int GetHashCode()
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.GetHashCode(this.FilePath ?? string.Empty)
                ^ comparer.GetHashCode(this.Name ?? string.Empty);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj"> An object to compare with this object. </param>
        /// <returns> <see langword="true"/> if the current object is equal to the <paramref name="obj"/> parameter; otherwise, <see langword="false"/>. </returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
            {
                return false;
            }
            
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = obj as AlternateDataStreamInfo;
            return !object.ReferenceEquals(null, other) && this.Equals(other);
        }

        /// <summary>
        /// Returns a value indicating whether
        /// this instance is equal to another instance.
        /// </summary>
        /// <param name="other"> The instance to compare to. </param>
        /// <returns> <see langword="true"/> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <see langword="false"/>. </returns>
        public bool Equals(AlternateDataStreamInfo other)
        {
            if (object.ReferenceEquals(null, other))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            var comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Equals(this.FilePath ?? string.Empty, other.FilePath ?? string.Empty)
                && comparer.Equals(this.Name ?? string.Empty, other.Name ?? string.Empty);
        }

        /// <summary>
        /// Deletes this stream from the parent file.
        /// </summary>
        /// <returns> <see langword="true"/> if the stream was deleted; otherwise, <see langword="false"/>. </returns>
        /// <exception cref="SecurityException"> The caller does not have the required permission.  </exception>
        /// <exception cref="UnauthorizedAccessException"> The caller does not have the required permission, or the file is read-only. </exception>
        /// <exception cref="IOException"> The specified file is in use.  </exception>
        /// <exception cref="ArgumentException"> The path of the stream is invalid. </exception>
        public bool Delete()
        {
            new FileIOPermission(FileIOPermissionAccess.Write, this.FilePath).Demand();
            return SafeNativeMethods.SafeDeleteFile(this.FullPath);
        }

        /// <summary>
        /// Opens this alternate data stream.
        /// </summary>
        /// <param name="mode"> A <see cref="FileMode"/> value that specifies whether a stream is created if one does not exist,  and determines whether the contents of existing streams are retained or overwritten. </param>
        /// <param name="access"> A <see cref="FileAccess"/> value that specifies the operations that can be performed on the stream.  </param>
        /// <param name="share"> A <see cref="FileShare"/> value specifying the type of access other threads have to the file.  </param>
        /// <param name="bufferSize"> The size of the buffer to use. </param>
        /// <param name="useAsync"> <see langword="true"/> to enable async-IO; otherwise, <see langword="false"/>. </param>
        /// <returns> A <see cref="FileStream"/> for this alternate data stream. </returns>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="bufferSize"/> is less than or equal to zero. </exception>
        /// <exception cref="SecurityException"> The caller does not have the required permission.  </exception>
        /// <exception cref="UnauthorizedAccessException"> The caller does not have the required permission, or the file is read-only. </exception>
        /// <exception cref="IOException"> The specified file is in use.  </exception>
        /// <exception cref="ArgumentException"> The path of the stream is invalid. </exception>
        /// <exception cref="Win32Exception"> There was an error opening the stream. </exception>
        public FileStream Open(FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
        {
            if (0 >= bufferSize)
            {
                throw new ArgumentOutOfRangeException("bufferSize", bufferSize, null);
            }

            var permAccess = CalculateAccess(mode, access);
            new FileIOPermission(permAccess, this.FilePath).Demand();

            var flags = useAsync ? SafeNativeMethods.NativeFileFlags.Overlapped : 0;
            var handle = SafeNativeMethods.SafeCreateFile(this.FullPath, access.ToNative(), share, IntPtr.Zero, mode, flags, IntPtr.Zero);
            if (handle.IsInvalid)
            {
                SafeNativeMethods.ThrowLastIOError(this.FullPath);
            }

            return new FileStream(handle, access, bufferSize, useAsync);
        }

        /// <summary>
        /// Opens this alternate data stream.
        /// </summary>
        /// <param name="mode">
        /// A <see cref="FileMode"/> value that specifies whether a stream is created if one does not exist, 
        /// and determines whether the contents of existing streams are retained or overwritten.
        /// </param>
        /// <param name="access">
        /// A <see cref="FileAccess"/> value that specifies the operations that can be performed on the stream. 
        /// </param>
        /// <param name="share">
        /// A <see cref="FileShare"/> value specifying the type of access other threads have to the file. 
        /// </param>
        /// <param name="bufferSize">
        /// The size of the buffer to use.
        /// </param>
        /// <returns>
        /// A <see cref="FileStream"/> for this alternate data stream.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="bufferSize"/> is less than or equal to zero.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission, or the file is read-only.
        /// </exception>
        /// <exception cref="IOException">
        /// The specified file is in use. 
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The path of the stream is invalid.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// There was an error opening the stream.
        /// </exception>
        public FileStream Open(FileMode mode, FileAccess access, FileShare share, int bufferSize)
        {
            return this.Open(mode, access, share, bufferSize, false);
        }

        /// <summary>
        /// Opens this alternate data stream.
        /// </summary>
        /// <param name="mode">
        /// A <see cref="FileMode"/> value that specifies whether a stream is created if one does not exist, 
        /// and determines whether the contents of existing streams are retained or overwritten.
        /// </param>
        /// <param name="access">
        /// A <see cref="FileAccess"/> value that specifies the operations that can be performed on the stream. 
        /// </param>
        /// <param name="share">
        /// A <see cref="FileShare"/> value specifying the type of access other threads have to the file. 
        /// </param>
        /// <returns>
        /// A <see cref="FileStream"/> for this alternate data stream.
        /// </returns>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission, or the file is read-only.
        /// </exception>
        /// <exception cref="IOException">
        /// The specified file is in use. 
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The path of the stream is invalid.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// There was an error opening the stream.
        /// </exception>
        public FileStream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return this.Open(mode, access, share, SafeNativeMethods.DefaultBufferSize, false);
        }

        /// <summary>
        /// Opens this alternate data stream.
        /// </summary>
        /// <param name="mode">
        /// A <see cref="FileMode"/> value that specifies whether a stream is created if one does not exist, 
        /// and determines whether the contents of existing streams are retained or overwritten.
        /// </param>
        /// <param name="access">
        /// A <see cref="FileAccess"/> value that specifies the operations that can be performed on the stream. 
        /// </param>
        /// <returns>
        /// A <see cref="FileStream"/> for this alternate data stream.
        /// </returns>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission, or the file is read-only.
        /// </exception>
        /// <exception cref="IOException">
        /// The specified file is in use. 
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The path of the stream is invalid.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// There was an error opening the stream.
        /// </exception>
        public FileStream Open(FileMode mode, FileAccess access)
        {
            return this.Open(mode, access, FileShare.None, SafeNativeMethods.DefaultBufferSize, false);
        }

        /// <summary>
        /// Opens this alternate data stream.
        /// </summary>
        /// <param name="mode">
        /// A <see cref="FileMode"/> value that specifies whether a stream is created if one does not exist, 
        /// and determines whether the contents of existing streams are retained or overwritten.
        /// </param>
        /// <returns>
        /// A <see cref="FileStream"/> for this alternate data stream.
        /// </returns>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission, or the file is read-only.
        /// </exception>
        /// <exception cref="IOException">
        /// The specified file is in use. 
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The path of the stream is invalid.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// There was an error opening the stream.
        /// </exception>
        public FileStream Open(FileMode mode)
        {
            FileAccess access = (FileMode.Append == mode) ? FileAccess.Write : FileAccess.ReadWrite;
            return this.Open(mode, access, FileShare.None, SafeNativeMethods.DefaultBufferSize, false);
        }

        /// <summary>
        /// Opens this stream for reading.
        /// </summary>
        /// <returns>
        /// A read-only <see cref="FileStream"/> for this stream.
        /// </returns>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission, or the file is read-only.
        /// </exception>
        /// <exception cref="IOException">
        /// The specified file is in use. 
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The path of the stream is invalid.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// There was an error opening the stream.
        /// </exception>
        public FileStream OpenRead()
        {
            return this.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// Opens this stream for writing.
        /// </summary>
        /// <returns>
        /// A write-only <see cref="FileStream"/> for this stream.
        /// </returns>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission, or the file is read-only.
        /// </exception>
        /// <exception cref="IOException">
        /// The specified file is in use. 
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The path of the stream is invalid.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// There was an error opening the stream.
        /// </exception>
        public FileStream OpenWrite()
        {
            return this.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        }

        /// <summary>
        /// Opens this stream as a text file.
        /// </summary>
        /// <returns>
        /// A <see cref="StreamReader"/> which can be used to read the contents of this stream.
        /// </returns>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission, or the file is read-only.
        /// </exception>
        /// <exception cref="IOException">
        /// The specified file is in use. 
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The path of the stream is invalid.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// There was an error opening the stream.
        /// </exception>
        public StreamReader OpenText()
        {
            Stream fileStream = this.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            return new StreamReader(fileStream);
        }

        /// <summary>
        /// Calculates the access to demand.
        /// </summary>
        /// <param name="mode"> The <see cref="FileMode"/>. </param>
        /// <param name="access"> The <see cref="FileAccess"/>. </param>
        /// <returns> The <see cref="FileIOPermissionAccess"/>. </returns>
        private static FileIOPermissionAccess CalculateAccess(FileMode mode, FileAccess access)
        {
            var permAccess = FileIOPermissionAccess.NoAccess;
            switch (mode)
            {
                case FileMode.Append:
                    permAccess = FileIOPermissionAccess.Append;
                    break;

                case FileMode.Create:
                case FileMode.CreateNew:
                case FileMode.OpenOrCreate:
                case FileMode.Truncate:
                    permAccess = FileIOPermissionAccess.Write;
                    break;

                case FileMode.Open:
                    permAccess = FileIOPermissionAccess.Read;
                    break;
            }

            switch (access)
            {
                case FileAccess.ReadWrite:
                    permAccess |= FileIOPermissionAccess.Write;
                    permAccess |= FileIOPermissionAccess.Read;
                    break;

                case FileAccess.Write:
                    permAccess |= FileIOPermissionAccess.Write;
                    break;

                case FileAccess.Read:
                    permAccess |= FileIOPermissionAccess.Read;
                    break;
            }

            return permAccess;
        }
    }
}
