using System;
using System.IO;

namespace LevelUp.Serializer
{
    public static class ObjectExtension
    {
        #region Methods

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static String Serialize<T>(this T obj, SerializerType type)
        {
            return Serializer.SerializeToText(obj, type);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="file">The file.</param>
        /// <param name="type">The type.</param>
        public static void Serialize<T>(this T obj, String file, SerializerType type)
        {
            Serializer.SerializeToFile(obj, file, type);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="type">The type.</param>
        public static void Serialize<T>(this T obj, Stream stream, SerializerType type)
        {
            Serializer.SerializeToStream(obj, stream, type);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="file">The file.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="type">The type.</param>
        public static void Serialize<T>(this T obj, String file, Int32 bufferSize, SerializerType type)
        {
            Serializer.SerializeToFile(obj, file, bufferSize, type);
        }

        /// <summary>
        /// Serializes to binary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="file">The file.</param>
        public static void ToBinary<T>(this T obj, String file)
        {
            Serializer.SerializeToFile(obj, file, SerializerType.Binary);
        }

        /// <summary>
        /// Serializes to binary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="stream">The stream.</param>
        public static void ToBinary<T>(this T obj, Stream stream)
        {
            Serializer.SerializeToStream(obj, stream, SerializerType.Binary);
        }

        /// <summary>
        /// Serializes to binary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="file">The file.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        public static void ToBinary<T>(this T obj, String file, Int32 bufferSize)
        {
            Serializer.SerializeToFile(obj, file, bufferSize, SerializerType.Binary);
        }

        /// <summary>
        /// Serializes to json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static String ToJSON<T>(this T obj)
        {
            return Serializer.SerializeToText(obj, SerializerType.Json);
        }

        /// <summary>
        /// Serializes to json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="file">The file.</param>
        public static void ToJSON<T>(this T obj, String file)
        {
            Serializer.SerializeToFile(obj, file, SerializerType.Json);
        }

        /// <summary>
        /// Serializes to json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="stream">The stream.</param>
        public static void ToJSON<T>(this T obj, Stream stream)
        {
            Serializer.SerializeToStream(obj, stream, SerializerType.Json);
        }

        /// <summary>
        /// Serializes to json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="file">The file.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        public static void ToJSON<T>(this T obj, String file, Int32 bufferSize)
        {
            Serializer.SerializeToFile(obj, file, bufferSize, SerializerType.Json);
        }

        /// <summary>
        /// Serializes to XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static String ToXML<T>(this T obj)
        {
            return Serializer.SerializeToText(obj, SerializerType.Xml);
        }

        /// <summary>
        /// Serializes to XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="file">The file.</param>
        public static void ToXML<T>(this T obj, String file)
        {
            Serializer.SerializeToFile(obj, file, SerializerType.Xml);
        }

        /// <summary>
        /// Serializes to XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="stream">The stream.</param>
        public static void ToXML<T>(this T obj, Stream stream)
        {
            Serializer.SerializeToStream(obj, stream, SerializerType.Xml);
        }

        /// <summary>
        /// Serializes to XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="file">The file.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        public static void ToXML<T>(this T obj, String file, Int32 bufferSize)
        {
            Serializer.SerializeToFile(obj, file, bufferSize, SerializerType.Xml);
        }

        #endregion Methods
    }
}