using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp;

namespace ConsoleApp1
{
    public class ZstdCompressionWithEncryption
    {
        public static void CompressFolderWithPassword(string folderPath, string outputPath, string password)
        {
            using (var outFile = File.Create(outputPath))
            using (var archiveStream = new MemoryStream())
            {
                // Compress the folder
                foreach (var filePath in Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories))
                {
                    string relativePath = Path.GetRelativePath(folderPath, filePath);
                    byte[] relativePathBytes = Encoding.UTF8.GetBytes(relativePath);

                    // Write file path to the archive
                    archiveStream.Write(BitConverter.GetBytes(relativePathBytes.Length), 0, 4);
                    archiveStream.Write(relativePathBytes, 0, relativePathBytes.Length);

                    // Compress the file content
                    byte[] fileContent = File.ReadAllBytes(filePath);

                    byte[] compressedContent = EasyCompressor.ZstdSharpCompressor.Shared.Compress(fileContent);
                    archiveStream.Write(BitConverter.GetBytes(compressedContent.Length), 0, 4);
                    archiveStream.Write(compressedContent, 0, compressedContent.Length);
                    
                }

                // Encrypt the compressed data with AES using the password
                byte[] encryptedData = Encrypt(archiveStream.ToArray(), password);
                outFile.Write(encryptedData, 0, encryptedData.Length);
            }

            Console.WriteLine("Folder compression and encryption completed.");
        }

        private static byte[] Encrypt(byte[] data, string password)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] key = CreateKey(password, aes.KeySize / 8);
                aes.Key = key;
                aes.GenerateIV();
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length); // Prepend IV for later use in decryption
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                    }
                    return ms.ToArray();
                }
            }
        }

        private static byte[] CreateKey(string password, int keySize)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password)).AsSpan(0, keySize).ToArray();
            }
        }
    }
}
