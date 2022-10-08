using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Mediska.Classes
{
    public class Hash
    {
        public enum enmHashAlgorithm
        {
            MD5,
            SHA1,
            SHA256,
            SHA384,
            SHA512
        }

        public static string ComputeHash(string InputString, enmHashAlgorithm HashAlgorithm, byte[] saltBytes)
        {
            // If salt is not specified, generate it on the fly.
            if (saltBytes is null)
            {

                // Define min and max salt sizes.
                int minSaltSize;
                int maxSaltSize;

                minSaltSize = 4;
                maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random;
                random = new Random();

                int saltSize;
                saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng;
                rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes;
            plainTextBytes = Encoding.Unicode.GetBytes(InputString);

            // Allocate array, which will hold plain text and salt.
            var plainTextWithSaltBytes = new byte[(plainTextBytes.Length + saltBytes.Length)];

            // Copy plain text bytes into resulting array.
            int I;
            var loopTo = plainTextBytes.Length - 1;
            for (I = 0; I <= loopTo; I++)
                plainTextWithSaltBytes[I] = plainTextBytes[I];

            // Append salt bytes to the resulting array.
            var loopTo1 = saltBytes.Length - 1;
            for (I = 0; I <= loopTo1; I++)
                plainTextWithSaltBytes[plainTextBytes.Length + I] = saltBytes[I];

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            var hash = default(HashAlgorithm);

            // Initialize appropriate hashing algorithm class.
            switch (HashAlgorithm)
            {

                case var @case when @case == enmHashAlgorithm.SHA1:
                    {
                        hash = new SHA1Managed();
                        break;
                    }

                case var case1 when case1 == enmHashAlgorithm.SHA256:
                    {
                        hash = new SHA256Managed();
                        break;
                    }

                case var case2 when case2 == enmHashAlgorithm.SHA384:
                    {
                        hash = new SHA384Managed();
                        break;
                    }

                case var case3 when case3 == enmHashAlgorithm.SHA512:
                    {
                        hash = new SHA512Managed();
                        break;
                    }

                case var case4 when case4 == enmHashAlgorithm.MD5:
                    {
                        hash = new MD5CryptoServiceProvider();
                        break;
                    }

            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes;
            hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            var hashWithSaltBytes = new byte[(hashBytes.Length + saltBytes.Length)];

            // Copy hash bytes into resulting array.
            var loopTo2 = hashBytes.Length - 1;
            for (I = 0; I <= loopTo2; I++)
                hashWithSaltBytes[I] = hashBytes[I];

            // Append salt bytes to the result.
            var loopTo3 = saltBytes.Length - 1;
            for (I = 0; I <= loopTo3; I++)
                hashWithSaltBytes[hashBytes.Length + I] = saltBytes[I];

            // Convert result into a base64-encoded string.
            string hashValue;
            hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        public static bool VerifyHash(string InputText, enmHashAlgorithm HashAlgorithm, string HashValue)
        {
            try
            {

  
            bool VerifyHashRet = default;
            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes;
            hashWithSaltBytes = Convert.FromBase64String(HashValue);

            // We must know size of hash (without salt).
            int hashSizeInBits;
            int hashSizeInBytes;

            // Size of hash is based on the specified algorithm.
            switch (HashAlgorithm)
            {

                case var @case when @case == enmHashAlgorithm.SHA1:
                    {
                        hashSizeInBits = 160;
                        break;
                    }

                case var case1 when case1 == enmHashAlgorithm.SHA256:
                    {
                        hashSizeInBits = 256;
                        break;
                    }

                case var case2 when case2 == enmHashAlgorithm.SHA384:
                    {
                        hashSizeInBits = 384;
                        break;
                    }

                case var case3 when case3 == enmHashAlgorithm.SHA512:
                    {
                        hashSizeInBits = 512; // Must be MD5
                        break;
                    }

                default:
                    {
                        hashSizeInBits = 128;
                        break;
                    }

            }

            // Convert size of hash from bits to bytes.
            hashSizeInBytes = (int)Math.Round(hashSizeInBits / 8d);

            // Make sure that the specified hash value is long enough.
            if (hashWithSaltBytes.Length < hashSizeInBytes)
            {
                VerifyHashRet = false;
            }

            // Allocate array to hold original salt bytes retrieved from hash.
            var saltBytes = new byte[(hashWithSaltBytes.Length - hashSizeInBytes)];

            // Copy salt from the end of the hash to the new array.
            int I;
            var loopTo = saltBytes.Length - 1;
            for (I = 0; I <= loopTo; I++)
                saltBytes[I] = hashWithSaltBytes[hashSizeInBytes + I];

            // Compute a new hash string.
            string expectedHashString;
            expectedHashString = ComputeHash(InputText, HashAlgorithm, saltBytes);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            VerifyHashRet = (HashValue ?? "") == (expectedHashString ?? "");
            return VerifyHashRet;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }

    public static class Crypto
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }


}