using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;

namespace LevelUp.Serializer
{
    public static class Serializer
    {
        #region Static Var

        private static Dictionary<SerializerType, ISerializer> _pool;
        private static Dictionary<SerializerType, Type> _preferTypePool;

        #endregion Static Var

        #region Private Static Property

        /// <summary>
        /// Gets the pool.
        /// </summary>
        /// <value>The pool.</value>
        /// <returns></returns>
        /// <remarks></remarks>
        private static Dictionary<SerializerType, ISerializer> m_Pool
        {
            get { return _pool ?? (_pool = new Dictionary<SerializerType, ISerializer>(Enum.GetNames(typeof(SerializerType)).Length)); }
        }

        private static Dictionary<SerializerType, Type> m_PreferTypePool
        {
            get { return _preferTypePool ?? (_preferTypePool = new Dictionary<SerializerType, Type>()); }
        }

        #endregion Private Static Property

        #region Public Static Method

        /// <summary>
        /// Get the serializer.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        public static ISerializer GetSerializer(SerializerType type)
        {
            var serializer = default(ISerializer);
            if (m_Pool.TryGetValue(type, out serializer)) return serializer;
            if (type == SerializerType.None)
                throw new ArgumentOutOfRangeException("type");

            if (!Enum.IsDefined(typeof(SerializerType), type))
                throw new ArgumentOutOfRangeException("type");

            var serializerType = default(Type);
            if (m_PreferTypePool.ContainsKey(type))
            {
                serializerType = m_PreferTypePool[type];
            }
            else
            {
                var attribute = type.GetCustomAttribute<RelativedTypeAttribute>();

                Contract.Assert(attribute != null,
               String.Format("{0} should be append with RelativedTypeAttribute",
                   Enum.GetName(typeof(SerializerType), type)));

                serializerType = attribute.Type;
            }

            serializer = Activator.CreateInstance(serializerType) as ISerializer;
            m_Pool[type] = serializer;

            return serializer;
        }

        public static void RegisterSerializer(SerializerType type, Type serializerType)
        {
            m_PreferTypePool[type] = serializerType;
        }

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="file">The file.</param>
        /// <param name="type">The type.</param>
        public static void SerializeToFile<T>(T obj, String file, SerializerType type)
        {
            GetSerializer(type).SerializeToFile(obj, file);
        }

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="file">The file.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="type">The type.</param>
        public static void SerializeToFile<T>(T obj, String file, Int32 bufferSize, SerializerType type)
        {
            GetSerializer(type).SerializeToFile(obj, file, bufferSize);
        }

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="type">The type.</param>
        public static void SerializeToStream<T>(T obj, Stream stream, SerializerType type)
        {
            GetSerializer(type).SerializeToStream(obj, stream);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static String SerializeToText<T>(T obj, SerializerType type)
        {
            return GetSerializer(type).SerializeToText(obj);
        }

        public static Byte[] SerializeToBytes<T>(T obj, SerializerType type)
        {
            return GetSerializer(type).SerializeToBytes(obj);
        }

        public static Byte[] SerializeToBytes<T>(T obj, bool enableCompress, SerializerType type)
        {
            return GetSerializer(type).SerializeToBytes(obj, enableCompress);
        }

        /// <summary>
        /// Des the serialize.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">The file.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static T DeSerializeFromFile<T>(String file, SerializerType type)
        {
            return GetSerializer(type).DeSerializeFromFile<T>(file);
        }

        /// <summary>
        /// Des the serialize.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">The file.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static T DeSerializeFromFile<T>(String file, Int32 bufferSize, SerializerType type)
        {
            return GetSerializer(type).DeSerializeFromFile<T>(file, bufferSize);
        }

        /// <summary>
        /// Des the serialize.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static T DeSerializeFromStream<T>(Stream stream, SerializerType type)
        {
            return GetSerializer(type).DeSerializeFromStream<T>(stream);
        }

        /// <summary>
        /// Des the serialize from text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text">The text.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static T DeSerializeFromText<T>(String text, SerializerType type)
        {
            return GetSerializer(type).DeSerializeFromText<T>(text);
        }

        public static T DeSerializeFromBytes<T>(byte[] bytes, bool enableDecompress, SerializerType type)
        {
            return GetSerializer(type).DeSerializeFromBytes<T>(bytes, enableDecompress);
        }

        #endregion Public Static Method
    }
}