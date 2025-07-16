using System;
using System.IO;
using System.Security.Cryptography;

namespace Prestarter.Helpers
{
    internal class HashProxyStream : Stream
    {
        private readonly Stream baseStream;
        private readonly HashAlgorithm hashAlgorithm;

        public HashProxyStream(Stream baseStream, HashAlgorithm hashAlgorithm)
        {
            this.baseStream = baseStream;
            this.hashAlgorithm = hashAlgorithm;
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new InvalidOperationException();

        public override long Position
        {
            get => throw new InvalidOperationException();
            set => throw new InvalidOperationException();
        }

        public override void Flush()
        {
            baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException();
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException();
        }

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
}