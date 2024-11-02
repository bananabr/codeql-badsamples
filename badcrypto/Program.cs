using System;
using System.Security.Cryptography;
using System.Text;
using System.Net.NetworkInformation;

namespace InsecureCryptoExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Using current time as key material
            var key1 = BitConverter.GetBytes(DateTime.Now.Ticks);
            EncryptData("Sensitive Data Example 1", key1);

            // 2. Using local machine's MAC address as key material
            var macAddress = GetMacAddress();
            var key2 = Encoding.UTF8.GetBytes(macAddress.Substring(0, 16)); // Truncate to 16 bytes if necessary
            EncryptData("Sensitive Data Example 2", key2);

            // 3. Using hash of machine name as key
            var machineName = Environment.MachineName;
            var key3 = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(machineName));
            EncryptData("Sensitive Data Example 3", key3);

            // 4. Using current date as IV
            var iv1 = BitConverter.GetBytes(DateTime.Today.ToBinary());
            EncryptData("Sensitive Data Example 4", key1, iv1);

            // 5. Using system uptime as key material
            var uptime = Environment.TickCount;
            var key4 = BitConverter.GetBytes(uptime);
            EncryptData("Sensitive Data Example 5", key4);

            // 6. Using predictable static seed for random key generation
            var rng = new Random(12345); // Predictable seed
            var key5 = new byte[16];
            rng.NextBytes(key5);
            EncryptData("Sensitive Data Example 6", key5);

            // 7. Using concatenation of predictable values for key
            var predictableConcatKey = Encoding.UTF8.GetBytes(macAddress + machineName).AsSpan(0, 16).ToArray();
            EncryptData("Sensitive Data Example 7", predictableConcatKey);

            // 8. Using process ID as IV
            var iv2 = BitConverter.GetBytes(Environment.ProcessId);
            EncryptData("Sensitive Data Example 8", key1, iv2);

            // 9. Using environment variable as key material
            var userKey = Environment.GetEnvironmentVariable("USERNAME");
            var key6 = Encoding.UTF8.GetBytes(userKey.PadRight(16, '0').Substring(0, 16));
            EncryptData("Sensitive Data Example 9", key6);

            // 10. Using fixed key across different encryptions
            var fixedKey = Encoding.UTF8.GetBytes("FixedEncryptionKey"); // Static key
            EncryptData("Sensitive Data Example 10", fixedKey);
        }

        static string GetMacAddress()
        {
            var mac = NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
            return mac;
        }

        static void EncryptData(string data, byte[] key, byte[] iv = null)
        {
            if (iv == null)
            {
                iv = new byte[16];
                new Random().NextBytes(iv); // Still not secure in real use
            }

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                var plainText = Encoding.UTF8.GetBytes(data);
                
                var cipherText = encryptor.TransformFinalBlock(plainText, 0, plainText.Length);
                Console.WriteLine("Encrypted Data: " + BitConverter.ToString(cipherText));
            }
        }
    }
}
