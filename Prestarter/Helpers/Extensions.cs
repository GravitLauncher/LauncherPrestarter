using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Prestarter.Helpers
{
    internal class HashProxyStream : Stream
    {
        private readonly Stream baseStream;
        private readonly HashAlgorithm hashAlgorithm;

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new InvalidOperationException();

        public override long Position { get => throw new InvalidOperationException(); set => throw new InvalidOperationException(); }

        public HashProxyStream(Stream baseStream, HashAlgorithm hashAlgorithm) 
        {
            this.baseStream = baseStream;
            this.hashAlgorithm = hashAlgorithm;
        }

        public override void Flush()
        {
            baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count) => throw new InvalidOperationException();

        public override long Seek(long offset, SeekOrigin origin) => throw new InvalidOperationException();

        public override void SetLength(long value) => throw new InvalidOperationException();

        public override void Write(byte[] buffer, int offset, int count)
        {
            hashAlgorithm.TransformBlock(buffer, offset, count, null, 0);
            baseStream.Write(buffer, offset, count);
        }

        public byte[] GetHash()
        {
            hashAlgorithm.TransformFinalBlock(new byte[0], 0, 0);
            return hashAlgorithm.Hash;
        }
    }

    public static class Extensions
    {
        public static void Download(this HttpClient client, string requestUri, Stream destination, Action<float> progress)
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

                    download.CopyTo(destination, 81920, totalBytes => progress?.Invoke((float)totalBytes / contentLength.Value));
                    progress?.Invoke(1);
                }
            }
        }

        public static void DownloadWithHash(this HttpClient client, string requestUri, string hashUri, HashAlgorithm hashAlgorithm,
            Stream destination, Action<float> progress)
        {
            var hash = client.GetStringAsync(hashUri).Result;
            var hashingProxyStream = new HashProxyStream(destination, hashAlgorithm);
            client.Download(requestUri, destination, progress);
            var downloadedHash = string.Join("", hashingProxyStream.GetHash().Select(item => item.ToString("x2")));
            var originalHash = hash.Trim().Split(' ')[0].Trim().ToLower();
            if (downloadedHash == originalHash)
            {
                throw new Exception($"Хеш-сумма не совпадает: {downloadedHash} != {originalHash}");
            }
        }

        public static void CopyTo(this Stream source, Stream destination, int bufferSize, Action<long> progress)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (!source.CanRead)
            {
                throw new ArgumentException("Has to be readable", nameof(source));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            if (!destination.CanWrite)
            {
                throw new ArgumentException("Has to be writable", nameof(destination));
            }
            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

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
