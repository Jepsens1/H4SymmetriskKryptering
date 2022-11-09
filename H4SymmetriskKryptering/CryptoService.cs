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
        private SymmetricAlgorithm _symmetric;

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
        public byte[] GetKey()
        {
            return _symmetric.Key;
        }
        public byte[] GetIV()
        {
            return _symmetric.IV;
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
    }
}
