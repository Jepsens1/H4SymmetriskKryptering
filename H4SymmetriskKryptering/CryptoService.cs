using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace H4SymmetriskKryptering
{
    internal class CryptoService
    {
        SymmetricAlgorithm _symmetric;

        public CryptoService(string cipher)
        {
            Generate(cipher);
        }
        private void Generate(string cipher)
        {
            switch (cipher.ToLower())
            {
                case "des":
                    _symmetric = DES.Create();
                    _symmetric.Key = GenerateRandomByteArray(8);
                    _symmetric.IV = GenerateRandomByteArray(8);
                    break;
                case "3des":
                    _symmetric = TripleDES.Create();
                    _symmetric.Key = GenerateRandomByteArray(24);
                    _symmetric.IV = GenerateRandomByteArray(8);
                    break;
                case "aes":
                    _symmetric = Aes.Create();
                    _symmetric.Key = GenerateRandomByteArray(32);
                    _symmetric.IV = GenerateRandomByteArray(16);
                    break;
            }

        }
        public string GetKey()
        {
            return Convert.ToBase64String(_symmetric.Key);
        }
        public string GetIV()
        {
            return Convert.ToBase64String(_symmetric.IV);
        }
        private byte[] GenerateRandomByteArray(int size)
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[size];
                rng.GetBytes(buffer);
                return buffer;
            }
        }
        public byte[] Encrypt(byte[] mess)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, _symmetric.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(mess, 0, mess.Length);
            cs.Close();
            return ms.ToArray();
        }
        public byte[] Decrypt(byte[] mess)
        {
            byte[] plaintext = new byte[mess.Length];
            MemoryStream ms = new MemoryStream(mess);
            CryptoStream cs = new CryptoStream(ms, _symmetric.CreateDecryptor(), CryptoStreamMode.Read);
            cs.Read(plaintext, 0, mess.Length);
            cs.Close();
            return plaintext;

        }
        public string EncryptString(string plainText)
        {
            byte[] array;
            ICryptoTransform encryptor = _symmetric.CreateEncryptor(_symmetric.Key, _symmetric.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }
            }


            return Convert.ToBase64String(array);
        }
        public string DecryptString(string cipherText)
        {
            byte[] buffer = Convert.FromBase64String(cipherText);
            ICryptoTransform decryptor = _symmetric.CreateDecryptor(_symmetric.Key, _symmetric.IV);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }

        }
    }
}
