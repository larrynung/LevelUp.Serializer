using Jil;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace LevelUp.Serializer.Jil
{
    /// <summary>
    ///
    /// </summary>
    public class JilSerializer : SerializerBase
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

            if (enableCompress)
                throw new NotImplementedException();

            var data = JSON.Serialize(obj);

            using (var sw = new StreamWriter(stream))
            {
                sw.Write(data);
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
        public override T DeSerializeFromStream<T>(Stream stream, bool enableDecompress = false
)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (!stream.CanRead)
                throw new ArgumentException("UnReadable stream.");

            if (enableDecompress)
                throw new NotImplementedException();

            using (var sr = new StreamReader(stream))
            {
                return JSON.Deserialize<T>(sr.ReadToEnd());
            }
        }

        #endregion Public Method
    }
}