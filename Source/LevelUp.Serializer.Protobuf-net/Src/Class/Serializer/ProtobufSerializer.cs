using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;

namespace LevelUp.Serializer.ProtobufNet
{
    /// <summary>
    ///
    /// </summary>
    public class ProtobufSerializer : SerializerBase
    {
        #region Public Method

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="stream">The stream.</param>
        /// <exception cref="ArgumentException">UnWritable stram.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <see langword="null" />.</exception>
        [SuppressMessage("Usage", "CC0022")]
        public override void SerializeToStream<T>(T obj, Stream stream, bool enableCompress = false)
        {
            if (ReferenceEquals(obj, null))
                throw new ArgumentNullException("obj");

            if (stream == null)
                throw new ArgumentNullException("stream");

            if (!stream.CanWrite)
                throw new ArgumentException("UnWritable stram.");

            if (!enableCompress)
            {
                ProtoBuf.Serializer.Serialize(stream, obj);
                return;
            }

            using (var compressionStream = new GZipStream(stream, CompressionMode.Compress))
            {
                ProtoBuf.Serializer.Serialize(compressionStream, obj);
            }
        }

        /// <summary>
        /// DeSerialize.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">UnReadable stream.</exception>
        public override T DeSerializeFromStream<T>(Stream stream, bool enableDecompress = false)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (!stream.CanRead)
                throw new ArgumentException("UnReadable stream.");

            if (!enableDecompress)
            {
                return ProtoBuf.Serializer.Deserialize<T>(stream);
            }

            using (var decompressionStream = new GZipStream(stream, CompressionMode.Decompress))
            {
                return ProtoBuf.Serializer.Deserialize<T>(decompressionStream);
            }
        }

        public override string SerializeToText<T>(T obj)
        {
            using (var ms = new MemoryStream(1024))
            {
                SerializeToStream(obj, ms);

                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public override T DeSerializeFromText<T>(string text)
        {
            var buffer = Convert.FromBase64String(text);
            using (var ms = new MemoryStream(buffer))
            {
                return DeSerializeFromStream<T>(ms);
            }
        }

        #endregion Public Method
    }
}