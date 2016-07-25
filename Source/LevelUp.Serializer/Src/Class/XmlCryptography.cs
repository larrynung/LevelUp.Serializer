using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace LevelUp.Serializer
{
    public static class XmlCryptography
    {
        #region Private Static Var

        private static RijndaelManaged _defaultCryptographyKey;

        #endregion Private Static Var

        #region Private Static Property

        /// <summary>
        /// Gets the default cryptography key.
        /// </summary>
        /// <value>The default cryptography key.</value>
        /// <returns></returns>
        /// <remarks></remarks>
        private static RijndaelManaged DefaultCryptographyKey
        {
            get
            {
                if (_defaultCryptographyKey == null)
                {
                    InitialDefaultCryptographyKey();
                }
                return _defaultCryptographyKey;
            }
        }

        #endregion Private Static Property

        #region Private Static Method

        /// <summary>
        /// Gets the default salt.
        /// </summary>
        /// <returns></returns>
        private static String GetDefaultSalt()
        {
            //{AA5AC2CB-A387-4F3A-8A71-ABCDA9A14405}

            var buffer = new Byte[38];
            buffer[0] = 0x7b;
            buffer[1] = 0x41;
            buffer[2] = 0x41;
            buffer[3] = 0x35;
            buffer[4] = 0x41;
            buffer[5] = 0x43;
            buffer[6] = 0x32;
            buffer[7] = 0x43;
            buffer[8] = 0x42;
            buffer[9] = 0x2d;
            buffer[10] = 0x41;
            buffer[11] = 0x33;
            buffer[12] = 0x38;
            buffer[13] = 0x37;
            buffer[14] = 0x2d;
            buffer[15] = 0x34;
            buffer[16] = 0x46;
            buffer[17] = 0x33;
            buffer[18] = 0x41;
            buffer[19] = 0x2d;
            buffer[20] = 0x38;
            buffer[21] = 0x41;
            buffer[22] = 0x37;
            buffer[23] = 0x31;
            buffer[24] = 0x2d;
            buffer[25] = 0x41;
            buffer[26] = 0x42;
            buffer[27] = 0x43;
            buffer[28] = 0x44;
            buffer[29] = 0x41;
            buffer[30] = 0x39;
            buffer[31] = 0x41;
            buffer[32] = 0x31;
            buffer[33] = 0x34;
            buffer[34] = 0x34;
            buffer[35] = 0x30;
            buffer[36] = 0x35;
            buffer[37] = 0x7d;

            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Gets the default password.
        /// </summary>
        /// <returns></returns>
        private static String GetDefaultPassword()
        {
            //{97ED4B10-3126-4660-9C35-62759C467EEF}

            var buffer = new Byte[38];
            buffer[0] = 0x7b;
            buffer[1] = 0x39;
            buffer[2] = 0x37;
            buffer[3] = 0x45;
            buffer[4] = 0x44;
            buffer[5] = 0x34;
            buffer[6] = 0x42;
            buffer[7] = 0x31;
            buffer[8] = 0x30;
            buffer[9] = 0x2d;
            buffer[10] = 0x33;
            buffer[11] = 0x31;
            buffer[12] = 0x32;
            buffer[13] = 0x36;
            buffer[14] = 0x2d;
            buffer[15] = 0x34;
            buffer[16] = 0x36;
            buffer[17] = 0x36;
            buffer[18] = 0x30;
            buffer[19] = 0x2d;
            buffer[20] = 0x39;
            buffer[21] = 0x43;
            buffer[22] = 0x33;
            buffer[23] = 0x35;
            buffer[24] = 0x2d;
            buffer[25] = 0x36;
            buffer[26] = 0x32;
            buffer[27] = 0x37;
            buffer[28] = 0x35;
            buffer[29] = 0x39;
            buffer[30] = 0x43;
            buffer[31] = 0x34;
            buffer[32] = 0x36;
            buffer[33] = 0x37;
            buffer[34] = 0x45;
            buffer[35] = 0x45;
            buffer[36] = 0x46;
            buffer[37] = 0x7d;

            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Initials the default cryptography key.
        /// </summary>
        private static void InitialDefaultCryptographyKey()
        {
            _defaultCryptographyKey = GetCryptographyKey(GetDefaultSalt(), GetDefaultPassword());
        }

        #endregion Private Static Method

        #region Public Static Method

        /// <summary>
        /// Gets the cryptography key.
        /// </summary>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">An attempt was made to set the key to null. </exception>
        /// <exception cref="CryptographicException">The key size is invalid.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="cb " />is out of range. This parameter requires a non-negative number.</exception>
        public static RijndaelManaged GetCryptographyKey(String salt, String password)
        {
            var saltArray = Encoding.ASCII.GetBytes(salt);
            //salt為隨機位元組,導入到金鑰末端使得解密更困難
            using (var rfc2898 = new Rfc2898DeriveBytes(password, saltArray))
            {
                var rm = new RijndaelManaged();
                rm.Key = rfc2898.GetBytes(Convert.ToInt32(rm.KeySize / 8));
                rm.IV = rfc2898.GetBytes(Convert.ToInt32(rm.BlockSize / 8));

                return rm;
            }
        }

        /// <summary>
        /// Encrypts the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="CryptographicException">The specified algorithm is not supported for XML Encryption.</exception>
        /// <exception cref="ArgumentNullException">The <see cref="P:System.Security.Cryptography.Xml.CipherData.CipherValue" />  property was set to null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="cb " />is out of range. This parameter requires a non-negative number.</exception>
        public static String EncryptXML(String xml, String salt, String password)
        {
            return EncryptXML(xml, GetCryptographyKey(salt, password));
        }

        /// <summary>
        /// Encrypts the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="CryptographicException">The specified algorithm is not supported for XML Encryption.</exception>
        /// <exception cref="ArgumentNullException">The <see cref="P:System.Security.Cryptography.Xml.CipherData.CipherValue" />  property was set to null.</exception>
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        public static String EncryptXML(String xml, SymmetricAlgorithm key = null)
        {
            if (String.IsNullOrEmpty(xml))
                throw new ArgumentNullException("xml");

            if (key == null)
            {
                key = DefaultCryptographyKey;
            }

            var xmlDoc = default(XmlDocument);
            var element = default(XmlElement);
            var eXml = default(EncryptedXml);
            var encryptedElement = default(Byte[]);
            var edElement = default(EncryptedData);
            var encryptionMethod = string.Empty;

            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            element = xmlDoc.DocumentElement;

            eXml = new EncryptedXml();

            Contract.Assert(element != null, "element != null");
            encryptedElement = eXml.EncryptData(element, key, false);

            edElement = new EncryptedData { Type = EncryptedXml.XmlEncElementUrl };

            if (key is TripleDES)
            {
                encryptionMethod = EncryptedXml.XmlEncTripleDESUrl;
            }
            else if (key is DES)
            {
                encryptionMethod = EncryptedXml.XmlEncDESUrl;
            }
            if (key is Rijndael)
            {
                switch (key.KeySize)
                {
                    case 128:
                        encryptionMethod = EncryptedXml.XmlEncAES128Url;
                        break;

                    case 192:
                        encryptionMethod = EncryptedXml.XmlEncAES192Url;
                        break;

                    case 256:
                        encryptionMethod = EncryptedXml.XmlEncAES256Url;
                        break;
                }
            }
            else
            {
                // Throw an exception if the transform is not in the previous categories
                throw new CryptographicException("The specified algorithm is not supported for XML Encryption.");
            }

            edElement.EncryptionMethod = new EncryptionMethod(encryptionMethod);
            // Add the encrypted element data to the
            // EncryptedData object.
            edElement.CipherData.CipherValue = encryptedElement;

            EncryptedXml.ReplaceElement(element, edElement, false);

            return xmlDoc.OuterXml;
        }

        /// <summary>
        /// Decrypts the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="CryptographicException">The <paramref name="value" /> parameter does not contain a &lt;CypherData&gt; node.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value" /> provided is null.</exception>
        public static String DecryptXML(String xml, String salt, String password)
        {
            return DecryptXML(xml, GetCryptographyKey(salt, password));
        }

        /// <summary>
        /// Decrypts the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The <paramref name="value" /> provided is null.</exception>
        /// <exception cref="CryptographicException">The <paramref name="value" /> parameter does not contain a &lt;CypherData&gt; node.</exception>
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        public static String DecryptXML(String xml, SymmetricAlgorithm key = null)
        {
            if (String.IsNullOrEmpty(xml))
                throw new ArgumentNullException("xml");

            if (key == null)
            {
                key = DefaultCryptographyKey;
            }

            var xmlDoc = default(XmlDocument);
            var element = default(XmlElement);
            var edElement = default(EncryptedData);
            var exml = default(EncryptedXml);
            var rgbOutput = default(Byte[]);

            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            element = xmlDoc.DocumentElement;

            // Create an EncryptedData object and populate it.
            edElement = new EncryptedData();

            Contract.Assert(element != null, "element != null");
            edElement.LoadXml(element);
            // Create a new EncryptedXml object.
            exml = new EncryptedXml();

            // Decrypt the element using the symmetric key.
            rgbOutput = exml.DecryptData(edElement, key);
            // Replace the encryptedData element with the plaintext XML element.
            exml.ReplaceData(element, rgbOutput);

            return xmlDoc.OuterXml;
        }

        #endregion Public Static Method
    }
}