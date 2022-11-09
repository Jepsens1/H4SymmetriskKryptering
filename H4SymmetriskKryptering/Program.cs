﻿using System.Diagnostics;
using System.Text;

namespace H4SymmetriskKryptering
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Display("aes");
            Display("des");
            Display("3des");
            Console.ReadKey();
        }
        static void Display(string method)
        {
            Stopwatch watch = Stopwatch.StartNew();
            CryptoService crypto = new CryptoService(method);
            string encrypted = crypto.EncryptString("Hello World");
            Console.WriteLine($"Encrypted message: {encrypted}");
            //byte[] decrypted = crypto.Decrypt(encrypted);
            string decrypted = crypto.DecryptString(encrypted);
            watch.Stop();
            Console.WriteLine($"Decrypted message: {decrypted}\nKey: {crypto.GetKey()}" +
                $"\nIV:  {crypto.GetIV()}\nTime it Took: {watch.Elapsed.Milliseconds}ms\n");
        }
    }
}