using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace LevelUp.Serializer
{
    /// <summary>
    ///
    /// </summary>
    public class JsonSerializer : SerializerBase
    {
        #region Public Method

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="stream">The stream.</param>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">UnWritable stram.</exception>
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

            var serializer = new DataContractJsonSerializer(typeof(T));
            serializer.WriteObject(stream, obj);
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

            if (enableDecompress)
                throw new NotImplementedException();

            var serializer = new DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(stream);
        }

        #endregion Public Method
    }
}