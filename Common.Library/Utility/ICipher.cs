using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Helper.Utility
{
    /// <summary>Encryption algorithm logic interface specification.</summary>
    public interface ICipher
    {
        /// <summary>
        /// The initialization vector to use for the symmetric algorithm
        /// (Non-essential, But the size of AES encryption must be 128,192, or 256 bits(1 byte = 8 bits)).
        /// </summary>
        byte[] IV { get; set; }
        /// <summary>Use a specific algorithm to encrypt the data.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be encrypted.</param>
        /// <returns>The encrypted data.</returns>
        byte[] Encrypt(byte[] secretKey, byte[] data);
        /// <summary>Use a specific algorithm to encrypt the data.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be encrypted.</param>
        /// <returns>The encrypted data.</returns>
        byte[] Encrypt(string secretKey, byte[] data);

        /// <summary>Use a specific algorithm to decrypt the data.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be decrypted.</param>
        /// <returns>The decrypted data, which is the original plain text before encryption.</returns>
        byte[] Decrypt(byte[] secretKey, byte[] data);
        /// <summary>Use a specific algorithm to decrypt the data.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be decrypted.</param>
        /// <returns>The decrypted data, which is the original plain text before encryption.</returns>
        byte[] Decrypt(string secretKey, byte[] data);

    }

    public class CipherBase
    {
        /// <summary>Copy the specified range of the byte array.</summary>
        /// <param name="from"></param>
        /// <param name="index">start index</param>
        /// <param name="length">End position index (not including last index)</param>
        /// <returns>Return the specified subscript to the end position (not including the last subscript)</returns>
        protected byte[] CopyOfRange(byte[] from, int index, int length)
        {
            byte[] to = new byte[length];
            for (int i = index; i < length; i++)
            {
                to[i] = from[i];
            }
            return to;
        }
    }

    /// <summary>The default implementation of AES encryption algorithm.</summary>
    public class AESCipher : CipherBase, ICipher
    {
        /// <summary>
        /// The initialization vector to use for the symmetric algorithm,
        /// AES encryption, the size must be 128,192, or 256 bits(1 byte = 8 bits).
        /// </summary>
        public byte[] IV { get; set; }

        /// <summary>Encrypts data with the System.Security.Cryptography.Rijndael(AES) algorithm.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be encrypted.</param>
        /// <returns>The encrypted data.</returns>
        public byte[] Encrypt(byte[] secretKey, byte[] data)
        {
            byte[] key = secretKey;
            if (secretKey.Length > 16 && secretKey.Length < 24)
                key = CopyOfRange(secretKey, 0, 16);
            else if(secretKey.Length>24&& secretKey.Length<32)
                key = CopyOfRange(secretKey, 0, 24);
            else if (secretKey.Length > 32)
                key = CopyOfRange(secretKey, 0, 32);

            RijndaelManaged rm = new RijndaelManaged { Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };
            ICryptoTransform ctf = rm.CreateEncryptor(secretKey, IV);
            byte[] decryptdata = ctf.TransformFinalBlock(data, 0, data.Length);
            return decryptdata;
        }

        /// <summary>Encrypts data with the System.Security.Cryptography.Rijndael(AES) algorithm.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be encrypted.</param>
        /// <returns>The encrypted data.</returns>
        public byte[] Encrypt(string secretKey, byte[] data)
        {
            return Encrypt(Convert.FromBase64String(secretKey), data);
        }

        /// <summary>Decrypts data with the System.Security.Cryptography.Rijndael(AES) algorithm.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be decrypted.</param>
        /// <returns>The decrypted data, which is the original plain text before encryption.</returns>
        public byte[] Decrypt(byte[] secretKey, byte[] data)
        {
            byte[] key = secretKey;
            if (secretKey.Length > 16 && secretKey.Length < 24)
                key = CopyOfRange(secretKey, 0, 16);
            else if (secretKey.Length > 24 && secretKey.Length < 32)
                key = CopyOfRange(secretKey, 0, 24);
            else if (secretKey.Length > 32)
                key = CopyOfRange(secretKey, 0, 32);

            RijndaelManaged rm = new RijndaelManaged { Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };
            ICryptoTransform ctf = rm.CreateDecryptor(secretKey, IV);
            byte[] decryptdata = ctf.TransformFinalBlock(data, 0, data.Length);
            return decryptdata;
        }

        /// <summary>Decrypts data with the System.Security.Cryptography.Rijndael(AES) algorithm.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be decrypted.</param>
        /// <returns>The decrypted data, which is the original plain text before encryption.</returns>
        public byte[] Decrypt(string secretKey, byte[] data)
        {
            return Decrypt(Convert.FromBase64String(secretKey), data);
        }
    }

    /// <summary>The default implementation of RSA encryption algorithm.</summary>
    public class RSACipher : ICipher
    {
        /// <summary>The initialization vector to use for the symmetric algorithm(Non-essential).</summary>
        public byte[] IV { get; set; }

        /// <summary>Encrypts data with the System.Security.Cryptography.RSA algorithm.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be encrypted.</param>
        /// <returns>The encrypted data.</returns>
        public byte[] Encrypt(string secretKey, byte[] data)
        {
            byte[] encryptdata;
            // The name of the key container, keep the encryption and decryption consistent to decrypt successfully
            CspParameters param = new CspParameters() { KeyContainerName = secretKey };
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                // true to perform direct System.Security.Cryptography.RSA decryption using OAEP
                // padding (only available on a computer running Microsoft Windows XP or later);
                // otherwise, false to use PKCS#1 v1.5 padding.
                encryptdata = rsa.Encrypt(data, false);
            }
            return encryptdata;
        }
        /// <summary>Encrypts data with the System.Security.Cryptography.RSA algorithm.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be encrypted.</param>
        /// <returns>The encrypted data.</returns>
        public byte[] Encrypt(byte[] secretKey, byte[] data)
        {
            return Encrypt(Convert.ToBase64String(secretKey), data);
        }

        /// <summary>Decrypts data with the System.Security.Cryptography.RSA algorithm.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be decrypted.</param>
        /// <returns>The decrypted data, which is the original plain text before encryption.</returns>
        public byte[] Decrypt(string secretKey, byte[] data)
        {
            byte[] decryptdata;
            // The name of the key container, keep the encryption and decryption consistent to decrypt successfully
            CspParameters param = new CspParameters() { KeyContainerName = secretKey };
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                // true to perform direct System.Security.Cryptography.RSA decryption using OAEP
                // padding (only available on a computer running Microsoft Windows XP or later);
                // otherwise, false to use PKCS#1 v1.5 padding.
                decryptdata = rsa.Decrypt(data, false);
            }
            return decryptdata;
        }
        /// <summary>Decrypts data with the System.Security.Cryptography.RSA algorithm.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be decrypted.</param>
        /// <returns>The decrypted data, which is the original plain text before encryption.</returns>
        public byte[] Decrypt(byte[] secretKey, byte[] data)
        {
            return Decrypt(Convert.ToBase64String(secretKey), data);
        }

    }

    /// <summary>The default implementation of RSA encryption algorithm.</summary>
    public class DESCipher : CipherBase, ICipher
    {
        /// <summary>The initialization vector to use for the symmetric algorithm.</summary>
        public byte[] IV { get; set; }

        /// <summary>Encrypts data with the System.Security.Cryptography.DES algorithm.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be encrypted.</param>
        /// <returns>The encrypted data.</returns>
        public byte[] Encrypt(string secretKey, byte[] data)
        {
            return Encrypt(Convert.FromBase64String(secretKey), data);
        }
        /// <summary>Encrypts data with the System.Security.Cryptography.DES algorithm.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be encrypted.</param>
        /// <returns>The encrypted data.</returns>
        public byte[] Encrypt(byte[] secretKey, byte[] data)
        {
            byte[] decryptdata;
            using DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                // The key specified by DES can only be 8 bytes in size
                byte[] key = (secretKey.Length > 8) ? CopyOfRange(secretKey, 0, 8) : secretKey;
                ICryptoTransform decryptor = des.CreateEncryptor(key, IV);
                CryptoStream cst = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);
                cst.Write(data, 0, data.Length);
                cst.FlushFinalBlock();
                cst.Close();
                decryptdata = ms.ToArray();
            }
            return decryptdata;
        }

        /// <summary>Decrypts data with the System.Security.Cryptography.DES algorithm.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be decrypted.</param>
        /// <returns>The decrypted data, which is the original plain text before encryption.</returns>
        public byte[] Decrypt(string secretKey, byte[] data)
        {
            return Decrypt(Convert.FromBase64String(secretKey), data);
        }
        /// <summary>Decrypts data with the System.Security.Cryptography.DES algorithm.</summary>
        /// <param name="secretKey">The name of the key container, keep the encryption and decryption consistent to decrypt successfully</param>
        /// <param name="data">The data to be decrypted.</param>
        /// <returns>The decrypted data, which is the original plain text before encryption.</returns>
        public byte[] Decrypt(byte[] secretKey, byte[] data)
        {
            byte[] decryptdata;
            using DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                // The key specified by DES can only be 8 bytes in size
                byte[] key = (secretKey.Length > 8) ? CopyOfRange(secretKey, 0, 8) : secretKey;
                ICryptoTransform decryptor = des.CreateDecryptor(key, IV);
                CryptoStream cst = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);
                cst.Write(data, 0, data.Length);
                cst.FlushFinalBlock();
                cst.Close();
                decryptdata = ms.ToArray();
            }
            return decryptdata;
        }

    }

}
