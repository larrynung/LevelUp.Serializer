using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace LevelUp.Serializer.JsonNet
{
    /// <summary>
    ///
    /// </summary>
    public class JsonNetSerializer : SerializerBase
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

            var serializer = new Newtonsoft.Json.JsonSerializer();
            var jsonTextWriter = new JsonTextWriter(new StreamWriter(stream));
            serializer.Serialize(jsonTextWriter, obj);
            jsonTextWriter.Flush();
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

            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (var jsonTextReader = new JsonTextReader(new StreamReader(stream)))
            {
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }

        #endregion Public Method
    }
}