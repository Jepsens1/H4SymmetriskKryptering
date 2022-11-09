using System.Diagnostics;
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
            byte[] encrypted = crypto.Encrypt(Encoding.UTF8.GetBytes("Hello World"));
            Console.WriteLine($"Encrypted message: {Encoding.UTF8.GetString(encrypted)}");
            byte[] decrypted = crypto.Decrypt(encrypted);
            watch.Stop();
            byte[] key = crypto.GetKey();
            byte[] iv = crypto.GetIV();
            Console.WriteLine($"Decrypted message: {Encoding.UTF8.GetString(decrypted)}\nKey: {Encoding.UTF8.GetString(key)}" +
                $"\nIV:  {Encoding.UTF8.GetString(iv)}\nTime it Took: {watch.Elapsed.Milliseconds}ms\n");
        }
    }
}