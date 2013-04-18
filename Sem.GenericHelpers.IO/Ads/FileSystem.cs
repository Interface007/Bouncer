// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystem.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   File-system utilities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.IO.Ads
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Security.Permissions;
    using System.Text;

    using Sem.GenericHelpers.IO.Properties;

    /// <summary>
    /// File-system utilities.
    /// </summary>
    public static class FileSystem
    {
        /// <summary>
        /// <span style="font-weight:bold;color:#a00;">(Extension Method)</span><br />
        /// Returns a read-only list of alternate data streams for the specified file.
        /// </summary>
        /// <param name="file">
        /// The <see cref="FileSystemInfo"/> to inspect.
        /// </param>
        /// <returns>
        /// A read-only list of <see cref="AlternateDataStreamInfo"/> objects
        /// representing the alternate data streams for the specified file, if any.
        /// If no streams are found, returns an empty list.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="file"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// The specified <paramref name="file"/> does not exist.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        public static IList<AlternateDataStreamInfo> ListAlternateDataStreams(this FileSystemInfo file)
        {
            if (null == file)
            {
                throw new ArgumentNullException("file");
            }

            if (!file.Exists)
            {
                throw new FileNotFoundException(null, file.FullName);
            }

            var path = file.FullName;
            new FileIOPermission(FileIOPermissionAccess.Read, path).Demand();

            return SafeNativeMethods.ListStreams(path)
                .Select(s => new AlternateDataStreamInfo(path, s))
                .ToList().AsReadOnly();
        }

        public static FileSystemInfo WriteAlternateDataStream(this FileSystemInfo file, string adsname, string text)
        {
            var ads = file.GetAlternateDataStream(adsname);
            using (var stream = ads.OpenWrite())
            {
                var bytes = Encoding.UTF8.GetBytes(text);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }

            return file;
        }

        public static string ReadAlternateDataStreamText(this FileSystemInfo file, string adsname)
        {
            var ads = file.GetAlternateDataStream(adsname);
            using (var stream = ads.OpenText())
            {
                return stream.ReadToEnd();
            }
        }

        /// <summary>
        /// Returns a read-only list of alternate data streams for the specified file.
        /// </summary>
        /// <param name="filePath">
        /// The full path of the file to inspect.
        /// </param>
        /// <returns>
        /// A read-only list of <see cref="AlternateDataStreamInfo"/> objects
        /// representing the alternate data streams for the specified file, if any.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filePath"/> is <see langword="null"/> or an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="filePath"/> is not a valid file path.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// The specified <paramref name="filePath"/> does not exist.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission.
        /// </exception>
        public static IList<AlternateDataStreamInfo> ListAlternateDataStreams(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");
            return CreateInfo(filePath).ListAlternateDataStreams();
        }

        /// <summary>
        /// <span style="font-weight:bold;color:#a00;">(Extension Method)</span><br />
        /// Returns a flag indicating whether the specified alternate data stream exists.
        /// </summary>
        /// <param name="file">
        /// The <see cref="FileInfo"/> to inspect.
        /// </param>
        /// <param name="streamName">
        /// The name of the stream to find.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the specified stream exists;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="file"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="streamName"/> contains invalid characters.
        /// </exception>
        public static bool AlternateDataStreamExists(this FileSystemInfo file, string streamName)
        {
            if (null == file) throw new ArgumentNullException("file");
            SafeNativeMethods.ValidateStreamName(streamName);

            string path = SafeNativeMethods.BuildStreamPath(file.FullName, streamName);
            return -1 != SafeNativeMethods.SafeGetFileAttributes(path);
        }

        /// <summary>
        /// Returns a flag indicating whether the specified alternate data stream exists.
        /// </summary>
        /// <param name="filePath">
        /// The path of the file to inspect.
        /// </param>
        /// <param name="streamName">
        /// The name of the stream to find.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the specified stream exists;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filePath"/> is <see langword="null"/> or an empty string.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="filePath"/> is not a valid file path.</para>
        /// <para>-or-</para>
        /// <para><paramref name="streamName"/> contains invalid characters.</para>
        /// </exception>
        public static bool AlternateDataStreamExists(string filePath, string streamName)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");
            return CreateInfo(filePath).AlternateDataStreamExists(streamName);
        }

        /// <summary>
        /// <span style="font-weight:bold;color:#a00;">(Extension Method)</span><br />
        /// Opens an alternate data stream.
        /// </summary>
        /// <param name="file">
        /// The <see cref="FileInfo"/> which contains the stream.
        /// </param>
        /// <param name="streamName">
        /// The name of the stream to open.
        /// </param>
        /// <param name="mode">
        /// One of the <see cref="FileMode"/> values, indicating how the stream is to be opened.
        /// </param>
        /// <returns>
        /// An <see cref="AlternateDataStreamInfo"/> representing the stream.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="file"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// The specified <paramref name="file"/> was not found.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="streamName"/> contains invalid characters.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="mode"/> is either <see cref="FileMode.Truncate"/> or <see cref="FileMode.Append"/>.
        /// </exception>
        /// <exception cref="IOException">
        /// <para><paramref name="mode"/> is <see cref="FileMode.Open"/>, and the stream doesn't exist.</para>
        /// <para>-or-</para>
        /// <para><paramref name="mode"/> is <see cref="FileMode.CreateNew"/>, and the stream already exists.</para>
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission, or the file is read-only.
        /// </exception>
        public static AlternateDataStreamInfo GetAlternateDataStream(this FileSystemInfo file, string streamName, FileMode mode)
        {
            if (null == file)
            {
                throw new ArgumentNullException("file");
            }

            if (!file.Exists)
            {
                throw new FileNotFoundException(null, file.FullName);
            }

            SafeNativeMethods.ValidateStreamName(streamName);

            if (FileMode.Truncate == mode || FileMode.Append == mode)
            {
                throw new NotSupportedException(string.Format(Resources.Culture, Resources.Error_InvalidMode, mode));
            }

            var permAccess = (FileMode.Open == mode) ? FileIOPermissionAccess.Read : FileIOPermissionAccess.Read | FileIOPermissionAccess.Write;
            new FileIOPermission(permAccess, file.FullName).Demand();

            string path = SafeNativeMethods.BuildStreamPath(file.FullName, streamName);
            bool exists = -1 != SafeNativeMethods.SafeGetFileAttributes(path);

            if (!exists && FileMode.Open == mode)
            {
                throw new IOException(string.Format(Resources.Culture, Resources.Error_StreamNotFound, streamName, file.Name));
            }
            if (exists && FileMode.CreateNew == mode)
            {
                throw new IOException(string.Format(Resources.Culture, Resources.Error_StreamExists, streamName, file.Name));
            }

            return new AlternateDataStreamInfo(file.FullName, streamName, path, exists);
        }

        /// <summary>
        /// <span style="font-weight:bold;color:#a00;">(Extension Method)</span><br />
        /// Opens an alternate data stream.
        /// </summary>
        /// <param name="file">
        /// The <see cref="FileInfo"/> which contains the stream.
        /// </param>
        /// <param name="streamName">
        /// The name of the stream to open.
        /// </param>
        /// <returns>
        /// An <see cref="AlternateDataStreamInfo"/> representing the stream.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="file"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// The specified <paramref name="file"/> was not found.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="streamName"/> contains invalid characters.
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission, or the file is read-only.
        /// </exception>
        public static AlternateDataStreamInfo GetAlternateDataStream(this FileSystemInfo file, string streamName)
        {
            return file.GetAlternateDataStream(streamName, FileMode.OpenOrCreate);
        }

        /// <summary>
        /// Opens an alternate data stream.
        /// </summary>
        /// <param name="filePath">
        /// The path of the file which contains the stream.
        /// </param>
        /// <param name="streamName">
        /// The name of the stream to open.
        /// </param>
        /// <param name="mode">
        /// One of the <see cref="FileMode"/> values, indicating how the stream is to be opened.
        /// </param>
        /// <returns>
        /// An <see cref="AlternateDataStreamInfo"/> representing the stream.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filePath"/> is <see langword="null"/> or an empty string.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// The specified <paramref name="filePath"/> was not found.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="filePath"/> is not a valid file path.</para>
        /// <para>-or-</para>
        /// <para><paramref name="streamName"/> contains invalid characters.</para>
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="mode"/> is either <see cref="FileMode.Truncate"/> or <see cref="FileMode.Append"/>.
        /// </exception>
        /// <exception cref="IOException">
        /// <para><paramref name="mode"/> is <see cref="FileMode.Open"/>, and the stream doesn't exist.</para>
        /// <para>-or-</para>
        /// <para><paramref name="mode"/> is <see cref="FileMode.CreateNew"/>, and the stream already exists.</para>
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission, or the file is read-only.
        /// </exception>
        public static AlternateDataStreamInfo GetAlternateDataStream(string filePath, string streamName, FileMode mode)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");
            return CreateInfo(filePath).GetAlternateDataStream(streamName, mode);
        }

        /// <summary>
        /// Opens an alternate data stream.
        /// </summary>
        /// <param name="filePath">
        /// The path of the file which contains the stream.
        /// </param>
        /// <param name="streamName">
        /// The name of the stream to open.
        /// </param>
        /// <returns>
        /// An <see cref="AlternateDataStreamInfo"/> representing the stream.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filePath"/> is <see langword="null"/> or an empty string.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// The specified <paramref name="filePath"/> was not found.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="filePath"/> is not a valid file path.</para>
        /// <para>-or-</para>
        /// <para><paramref name="streamName"/> contains invalid characters.</para>
        /// </exception>
        /// <exception cref="SecurityException">
        /// The caller does not have the required permission. 
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The caller does not have the required permission, or the file is read-only.
        /// </exception>
        public static AlternateDataStreamInfo GetAlternateDataStream(string filePath, string streamName)
        {
            return GetAlternateDataStream(filePath, streamName, FileMode.OpenOrCreate);
        }

        /// <summary>
        /// Creates a <see cref="FileSystemInfo"/> for the specified path.
        /// </summary>
        /// <param name="path">
        /// The path of the file or directory.
        /// </param>
        /// <returns>
        /// The <see cref="FileSystemInfo"/> representing the file or directory.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is <see langword="null"/> or empty.
        /// </exception>
        private static FileSystemInfo CreateInfo(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");

            path = Path.GetFullPath(path);
            if (!File.Exists(path) && Directory.Exists(path)) return new DirectoryInfo(path);
            return new FileInfo(path);
        }
    }
}
