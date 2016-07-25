using System;
using System.IO;
using System.Text;

namespace LevelUp.Serializer
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SerializerBase : ISerializer
    {
        #region Const

        private const Int32 BUFFER_SIZE = 1024;

        #endregion Const

        #region Method

        /// <summary>
        /// Des the serialize from text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public virtual T DeSerializeFromText<T>(String text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return DeSerializeFromBytes<T>(bytes);
        }

        /// <summary>
        /// Serializes to text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public virtual String SerializeToText<T>(T obj)
        {
            using (var ms = new MemoryStream(1024))
            {
                SerializeToStream(obj, ms);

                ms.Seek(0, SeekOrigin.Begin);

                using (var sr = new StreamReader(ms))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="file">The file.</param>
        public virtual void SerializeToFile<T>(T obj, String file)
        {
            SerializeToFile(obj, file, BUFFER_SIZE);
        }

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="file">The file.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is <see langword="null" />.</exception>
        public virtual void SerializeToFile<T>(T obj, String file, Int32 bufferSize)
        {
            if (ReferenceEquals(obj, null))
                throw new ArgumentNullException("obj");

            if (String.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");

            using (var fs = File.Open(file, FileMode.Create, FileAccess.Write))
            {
                using (var bs = new BufferedStream(fs, bufferSize))
                {
                    SerializeToStream(obj, bs);
                    bs.Flush();
                }
            }
        }

        public abstract void SerializeToStream<T>(T obj, Stream stream, bool enableCompress = false);

        /// <summary>
        /// Des the serialize.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public T DeSerializeFromFile<T>(String file)
        {
            return DeSerializeFromFile<T>(file, BUFFER_SIZE);
        }

        /// <summary>
        /// Des the serialize.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">The file.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="file"/> is <see langword="null" />.</exception>
        public virtual T DeSerializeFromFile<T>(String file, Int32 bufferSize)
        {
            if (String.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");

            using (var fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var bs = new BufferedStream(fs, bufferSize))
                {
                    return DeSerializeFromStream<T>(bs);
                }
            }
        }

        /// <summary>
        /// DeSerialize.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public abstract T DeSerializeFromStream<T>(Stream stream, bool enableDecompress = false);

        public virtual T DeSerializeFromBytes<T>(byte[] bytes, bool enableDecompress = false)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return DeSerializeFromStream<T>(ms, enableDecompress);
            }
        }

        public virtual byte[] SerializeToBytes<T>(T obj, bool enableCompress = false)
        {
            using (var ms = new MemoryStream())
            {
                SerializeToStream(obj, ms, enableCompress);
                return ms.ToArray();
            }
        }

        #endregion Method
    }
}