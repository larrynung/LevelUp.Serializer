using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace LevelUp.Serializer
{
    /// <summary>
    ///
    /// </summary>
    public class XmlSerializer : SerializerBase
    {
        #region Const

        private const String SERIALIZER_DLL_EXTENSION = ".XmlSerializers.dll";
        private const String SERIALIZER_DLL_NAMESPACE_PATTERN = "Microsoft.Xml.Serialization.GeneratedAssembly.{0}Serializer";
        private const Int32 POOL_BUFFER_SIZE = 25;
        private const Int32 BUFFER_SIZE = 1024;

        #endregion Const

        #region Var

        private static Dictionary<Type, System.Xml.Serialization.XmlSerializer> _pool;

        #endregion Var

        #region Private Static Property

        /// <summary>
        /// Gets the pool.
        /// </summary>
        /// <value>The pool.</value>
        /// <returns></returns>
        /// <remarks></remarks>
        private static Dictionary<Type, System.Xml.Serialization.XmlSerializer> m_Pool
        {
            get { return _pool ?? (_pool = new Dictionary<Type, System.Xml.Serialization.XmlSerializer>(POOL_BUFFER_SIZE)); }
        }

        #endregion Private Static Property

        #region Private Static Method

        /// <summary>
        /// Gets the XML serializer.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <returns>returns</returns>
        /// <remarks>remarks</remarks>
        public static System.Xml.Serialization.XmlSerializer GetXmlSerializer<T>()
        {
            return GetXmlSerializer(typeof(T));
        }

        /// <summary>
        /// Gets the XML serializer.
        /// </summary>
        /// <param name="objType">Type of the obj.</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        public static System.Xml.Serialization.XmlSerializer GetXmlSerializer(Type objType)
        {
            var serializer = default(System.Xml.Serialization.XmlSerializer);

            //lock (m_Pool)
            //{
            if (m_Pool.TryGetValue(objType, out serializer)) return serializer;
            var asmFile = objType.Assembly.Location;
            var serializerAssemblyFile = asmFile.Substring(0, asmFile.LastIndexOf(".", StringComparison.Ordinal)) +
                                         SERIALIZER_DLL_EXTENSION;
            if (File.Exists(serializerAssemblyFile))
            {
                var serializerAssembly = Assembly.LoadFile(serializerAssemblyFile);
                var serializerAssemblyObjType =
                    serializerAssembly.GetType(String.Format(SERIALIZER_DLL_NAMESPACE_PATTERN, objType.Name));

                serializer = serializerAssemblyObjType == null
                        ? new System.Xml.Serialization.XmlSerializer(objType)
                        : (System.Xml.Serialization.XmlSerializer)Activator.CreateInstance(serializerAssemblyObjType);
            }
            else
            {
                serializer = new System.Xml.Serialization.XmlSerializer(objType);
            }
            m_Pool[objType] = serializer;
            //}

            return serializer;
        }

        #endregion Private Static Method

        #region Public Method

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="file">The file.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> is <see langword="null" />.see</exception>
        public void SerializeToFile<T>(T obj, String file, String salt, String password, Int32 bufferSize = BUFFER_SIZE)
        {
            if (String.IsNullOrEmpty(salt)) throw new ArgumentNullException("salt");
            if (String.IsNullOrEmpty(password)) throw new ArgumentNullException("password");

            SerializeToFile(obj, file, true, XmlCryptography.GetCryptographyKey(salt, password), bufferSize);
        }

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="file">The file.</param>
        /// <param name="key">The key.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null" />.see</exception>
        public void SerializeToFile<T>(T obj, String file, SymmetricAlgorithm key, Int32 bufferSize = BUFFER_SIZE)
        {
            if (key == null) throw new ArgumentNullException("key");

            SerializeToFile(obj, file, true, key, bufferSize);
        }

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="file">The file.</param>
        /// <param name="needEncrypt">if set to <c>true</c> [need encrypt].c</param>
        /// <param name="key">The key.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is <see langword="null" />.see</exception>
        public void SerializeToFile<T>(T obj, String file, Boolean needEncrypt, SymmetricAlgorithm key = null, Int32 bufferSize = BUFFER_SIZE)
        {
            if (ReferenceEquals(obj, null))
                throw new ArgumentNullException("obj");

            if (String.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");

            using (var fs = File.Open(file, FileMode.Create, FileAccess.Write))
            {
                using (var bs = new BufferedStream(fs, bufferSize))
                {
                    SerializeToStream(obj, bs, needEncrypt, key);
                    bs.Flush();
                }
            }
        }

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="ArgumentNullException"><paramref name="salt"/> is <see langword="null" />.see</exception>
        public void SerializeToStream<T>(T obj, Stream stream, String salt, String password)
        {
            if (String.IsNullOrEmpty(salt)) throw new ArgumentNullException("salt");
            if (String.IsNullOrEmpty(password)) throw new ArgumentNullException("password");

            SerializeToStream(obj, stream, XmlCryptography.GetCryptographyKey(salt, password));
        }

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="key">The key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null" />.see</exception>
        public void SerializeToStream<T>(T obj, Stream stream, SymmetricAlgorithm key)
        {
            if (key == null) throw new ArgumentNullException("key");

            SerializeToStream(obj, stream, true, key);
        }

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="needEncrypt">if set to <c>true</c> [need encrypt].c</param>
        /// <param name="key">The key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is <see langword="null" />.see</exception>
        /// <exception cref="ArgumentException">UnWritable stram.</exception>
        public void SerializeToStream<T>(T obj, Stream stream, Boolean needEncrypt, SymmetricAlgorithm key = null)
        {
            if (!needEncrypt)
            {
                SerializeToStream(obj, stream);
                return;
            }

            if (ReferenceEquals(obj, null))
                throw new ArgumentNullException("obj");

            if (stream == null)
                throw new ArgumentNullException("stream");

            if (!stream.CanWrite)
                throw new ArgumentException("UnWritable stram.");

            using (var ms = new MemoryStream(BUFFER_SIZE))
            {
                var serializer = GetXmlSerializer<T>();
                serializer.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                var buffer = new Byte[Convert.ToInt32(ms.Length) + 1];
                ms.Read(buffer, 0, buffer.Length);
                var plantText = Encoding.UTF8.GetString(buffer);
                var encryptText = XmlCryptography.EncryptXML(plantText, key);

                var encryptBuffer = Encoding.UTF8.GetBytes(encryptText);
                stream.Write(encryptBuffer, 0, encryptBuffer.Length);
            }
        }

        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="stream">The stream.</param>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is <see langword="null" />.see</exception>
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

            var serializer = GetXmlSerializer<T>();
            serializer.Serialize(stream, obj);
        }

        /// <summary>
        /// Des the serialize.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <param name="file">The file.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <returns>returns</returns>
        /// <exception cref="ArgumentNullException"><paramref name="salt"/> is <see langword="null" />.see</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="cb " />is out of range. This parameter requires a non-negative number.</exception>
        public T DeSerializeFromFile<T>(String file, String salt, String password, Int32 bufferSize = BUFFER_SIZE)
        {
            if (String.IsNullOrEmpty(salt)) throw new ArgumentNullException("salt");
            if (String.IsNullOrEmpty(password)) throw new ArgumentNullException("password");

            return DeSerializeFromFile<T>(file, XmlCryptography.GetCryptographyKey(salt, password), bufferSize);
        }

        /// <summary>
        /// Des the serialize.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <param name="file">The file.</param>
        /// <param name="key">The key.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <returns>returns</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null" />.see</exception>
        public T DeSerializeFromFile<T>(String file, SymmetricAlgorithm key, Int32 bufferSize = BUFFER_SIZE)
        {
            if (key == null) throw new ArgumentNullException("key");

            return DeSerializeFromFile<T>(file, true, key, bufferSize);
        }

        /// <summary>
        /// Des the serialize.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <param name="file">The file.</param>
        /// <param name="needDecrypt">if set to <c>true</c> [need decrypt].c</param>
        /// <param name="key">The key.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <returns>returns</returns>
        /// <exception cref="ArgumentNullException"><paramref name="file"/> is <see langword="null" />.see</exception>
        public T DeSerializeFromFile<T>(String file, Boolean needDecrypt, SymmetricAlgorithm key = null, Int32 bufferSize = BUFFER_SIZE)
        {
            if (String.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");

            using (var fs = File.Open(file, FileMode.Open, FileAccess.Read))
            {
                using (var bs = new BufferedStream(fs, bufferSize))
                {
                    return DeSerializeFromStream<T>(bs, needDecrypt, key);
                }
            }
        }

        /// <summary>
        /// Des the serialize.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        /// <returns>returns</returns>
        /// <exception cref="ArgumentNullException"><paramref name="salt"/> is <see langword="null" />.see</exception>
        public T DeSerializeFromStream<T>(Stream stream, String salt, String password)
        {
            if (String.IsNullOrEmpty(salt)) throw new ArgumentNullException("salt");
            if (String.IsNullOrEmpty(password)) throw new ArgumentNullException("password");

            return DeSerializeFromStream<T>(stream, XmlCryptography.GetCryptographyKey(salt, password));
        }

        /// <summary>
        /// Des the serialize.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="key">The key.</param>
        /// <returns>returns</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null" />.see</exception>
        public T DeSerializeFromStream<T>(Stream stream, SymmetricAlgorithm key)
        {
            if (key == null) throw new ArgumentNullException("key");

            return DeSerializeFromStream<T>(stream, true, key);
        }

        /// <summary>
        /// Des the serialize.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="needDecrypt">if set to <c>true</c> [need decrypt].c</param>
        /// <param name="key">The key.</param>
        /// <returns>returns</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <see langword="null" />.see</exception>
        /// <exception cref="ArgumentException">UnReadable stream.</exception>
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        public T DeSerializeFromStream<T>(Stream stream, Boolean needDecrypt, SymmetricAlgorithm key = null)
        {
            if (!needDecrypt)
            {
                return DeSerializeFromStream<T>(stream);
            }

            if (stream == null)
                throw new ArgumentNullException("stream");

            if (!stream.CanRead)
                throw new ArgumentException("UnReadable stream.");

            using (var ms = new MemoryStream(BUFFER_SIZE))
            {
                var encryptText = String.Empty;
                using (var streamReader = new StreamReader(stream))
                {
                    encryptText = streamReader.ReadToEnd();
                }
                var plantText = XmlCryptography.DecryptXML(encryptText, key);
                var buffer = Encoding.UTF8.GetBytes(plantText);
                ms.Write(buffer, 0, buffer.Length);
                ms.Seek(0, SeekOrigin.Begin);

                return (T)GetXmlSerializer<T>().Deserialize(ms);
            }
        }

        /// <summary>
        /// DeSerialize.
        /// </summary>
        /// <typeparam name="T">typeparam</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>returns</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is <see langword="null" />.see</exception>
        /// <exception cref="ArgumentException">UnReadable stream.</exception>
        public override T DeSerializeFromStream<T>(Stream stream, bool enableDecompress = false)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (!stream.CanRead)
                throw new ArgumentException("UnReadable stream.");

            if (enableDecompress)
                throw new NotImplementedException();

            var serializer = GetXmlSerializer<T>();
            return (T)serializer.Deserialize(stream);
        }

        #endregion Public Method
    }
}