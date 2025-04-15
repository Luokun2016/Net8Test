using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp;

namespace ConsoleApp1
{
    public class ZstdDecompressionWithDecryption
    {
        public static void DecompressFolderWithPassword(string inputPath, string outputFolderPath, string password)
        {
            using (var inFile = File.OpenRead(inputPath))
            {
                byte[] encryptedData = new byte[inFile.Length];
                inFile.Read(encryptedData, 0, encryptedData.Length);

                // Decrypt the data
                byte[] decryptedData = Decrypt(encryptedData, password);
                using (var archiveStream = new MemoryStream(decryptedData))
                {
                    while (archiveStream.Position < archiveStream.Length)
                    {
                        // Extract relative file path
                        byte[] pathLengthBytes = new byte[4];
                        archiveStream.Read(pathLengthBytes, 0, 4);
                        int pathLength = BitConverter.ToInt32(pathLengthBytes, 0);

                        byte[] relativePathBytes = new byte[pathLength];
                        archiveStream.Read(relativePathBytes, 0, pathLength);
                        string relativePath = Encoding.UTF8.GetString(relativePathBytes);

                        // Read compressed file content
                        byte[] compressedSizeBytes = new byte[4];
                        archiveStream.Read(compressedSizeBytes, 0, 4);
                        int compressedSize = BitConverter.ToInt32(compressedSizeBytes, 0);

                        byte[] compressedContent = new byte[compressedSize];
                        archiveStream.Read(compressedContent, 0, compressedContent.Length);

                        // Decompress file content  
                        byte[] decompressedContent = EasyCompressor.ZstdSharpCompressor.Shared.Decompress(compressedContent);

                        // Write file to disk
                        string outputFilePath = Path.Combine(outputFolderPath, relativePath);
                        Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
                        File.WriteAllBytes(outputFilePath, decompressedContent);
                        
                    }
                }
            }

            Console.WriteLine("Folder decryption and decompression completed.");
        }

        private static byte[] Decrypt(byte[] encryptedData, string password)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] key = CreateKey(password, aes.KeySize / 8);

                // Extract IV from the start of the encrypted data
                byte[] iv = new byte[aes.BlockSize / 8];
                Array.Copy(encryptedData, iv, iv.Length);

                aes.Key = key;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedData, iv.Length, encryptedData.Length - iv.Length);
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
