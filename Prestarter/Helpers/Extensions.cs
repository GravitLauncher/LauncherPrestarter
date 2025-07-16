using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;

namespace Prestarter.Helpers
{
    internal static class Extensions
    {
        public static void Download(this HttpClient client, string requestUri, Stream destination,
            Action<float> progress)
        {
            using (var response = client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead).Result)
            {
                var contentLength = response.Content.Headers.ContentLength;

                using (var download = response.Content.ReadAsStreamAsync().Result)
                {
                    if (!contentLength.HasValue)
                    {
                        download.CopyTo(destination);
                        return;
                    }

                    download.CopyTo(destination, 81920,
                        totalBytes => progress?.Invoke((float)totalBytes / contentLength.Value));
                    progress?.Invoke(1);
                }
            }
        }

        public static void DownloadWithHashUrl(this HttpClient client, string requestUri, string hashUri,
            HashAlgorithm hashAlgorithm,
            Stream destination, Action<float> progress)
        {
            var hash = client.GetStringAsync(hashUri).Result;
            var originalHash = hash.Trim().Split(' ')[0].Trim().ToLower();
            client.DownloadWithHash(requestUri, originalHash, hashAlgorithm, destination, progress);
        }

        public static void DownloadWithHash(this HttpClient client, string requestUri, string hash,
            HashAlgorithm hashAlgorithm,
            Stream destination, Action<float> progress)
        {
            var hashingProxyStream = new HashProxyStream(destination, hashAlgorithm);
            client.Download(requestUri, destination, progress);
            var downloadedHash = string.Join("", hashingProxyStream.GetHash().Select(item => item.ToString("x2")));
            if (downloadedHash == hash)
                throw new Exception(string.Format(I18n.HashsumIsIncorrectError, downloadedHash, hash));
        }

        public static void CopyTo(this Stream source, Stream destination, int bufferSize, Action<long> progress)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (!source.CanRead) throw new ArgumentException("Has to be readable", nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite) throw new ArgumentException("Has to be writable", nameof(destination));
            if (bufferSize < 0) throw new ArgumentOutOfRangeException(nameof(bufferSize));

            var buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) != 0)
            {
                destination.Write(buffer, 0, bytesRead);
                totalBytesRead += bytesRead;
                progress?.Invoke(totalBytesRead);
            }
        }
    }
}