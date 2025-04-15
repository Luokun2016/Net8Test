using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace BrotliUtility.Utility
{
    public class FileUtility
    {
        public static string SelectFile(string filter = null)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "Custom File(*.txt, *.csv)|*.txt;*.csv"
            };

            if (!string.IsNullOrEmpty(filter))
            {
                openFileDialog.Filter = filter;
            }

            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string SelectFolder()
        {
            var folderBrowserDialog = new OpenFolderDialog();
            folderBrowserDialog.Multiselect = false;

            // Show open folder dialog box
            bool? result = folderBrowserDialog.ShowDialog();

            // Process open folder dialog box results
            if (result == true)
            {
                // Get the selected folder
                return folderBrowserDialog.FolderName;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string DecompressBrotliFile(string filePath)
        {
            var result = string.Empty;

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (BrotliStream brotliStream = new BrotliStream(fileStream, CompressionMode.Decompress))
                {
                    var obj = JsonObject.Parse(brotliStream);
                    result = JsonSerializer.Serialize(obj, new JsonSerializerOptions()
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                        WriteIndented = true
                    });
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }
    }
}
