using System;
using System.IO;

namespace LevelUp.Serializer
{
    /// <summary>
    ///
    /// </summary>
    public interface ISerializer
    {
        #region Methods

        /// <summary>
        /// Des the serialize from file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        T DeSerializeFromFile<T>(String file);

        /// <summary>
        /// Des the serialize from file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">The file.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <returns></returns>
        T DeSerializeFromFile<T>(String file, Int32 bufferSize);

        /// <summary>
        /// Des the serialize from stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        T DeSerializeFromStream<T>(Stream stream, bool enableDecompress = false);

        /// <summary>
        /// Des the serialize from text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        T DeSerializeFromText<T>(String text);

        T DeSerializeFromBytes<T>(byte[] bytes, bool enableDecompress = false);

        /// <summary>
        /// Serializes to file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="file">The file.</param>
        void SerializeToFile<T>(T obj, String file);

        /// <summary>
        /// Serializes to file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="file">The file.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        void SerializeToFile<T>(T obj, String file, Int32 bufferSize);

        /// <summary>
        /// Serializes to stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="stream">The stream.</param>
        void SerializeToStream<T>(T obj, Stream stream, bool enableCompress = false);

        String SerializeToText<T>(T obj);

        Byte[] SerializeToBytes<T>(T obj, bool enableCompress = false);

        #endregion Methods
    }
}