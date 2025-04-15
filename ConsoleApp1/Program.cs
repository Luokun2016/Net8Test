using System.Security.Principal;
using LevelDB;
using Newtonsoft.Json;
using Semver;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var current = SemVersion.Parse("1.12.2-dev.1", SemVersionStyles.Any);
            var minSupportVersion = SemVersion.Parse("1.12.1", SemVersionStyles.Any);

            var a22 = minSupportVersion.CompareSortOrderTo(current);

            /*// Get the current Windows identity
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            // Check if the identity is not null
            if (identity != null)
            {
                // Display the user's identity information
                Console.WriteLine("Name: " + identity.Name);
                Console.WriteLine("Authentication Type: " + identity.AuthenticationType);
                Console.WriteLine("Is Authenticated: " + identity.IsAuthenticated);
                Console.WriteLine("Is Guest: " + identity.IsGuest);
                Console.WriteLine("Is System: " + identity.IsSystem);
                Console.WriteLine("Is Anonymous: " + identity.IsAnonymous);
                Console.WriteLine("Owner SID: " + identity.Owner);
                Console.WriteLine("User SID: " + identity.User);
            }
            else
            {
                Console.WriteLine("Could not retrieve the current Windows identity.");
            }*/


            var options = new Options { CreateIfMissing = true };
            using var db = new DB(options, @"C:\Users\Allen\AppData\Roaming\fstudio-unified\Local Storage\leveldb1");
            db.Put("a", "aaaa");
            Console.WriteLine("Key: {0}, value:{1}", "a", db.Get("a"));
            var keys =
                    from kv in db as IEnumerable<IEnumerable<KeyValuePair<string, string>>>
                    select kv.ToList();

            var a12 = keys.ToList();
            /*foreach (var key in keys)
            {
                Console.WriteLine("Key: {0}, value:{1}", key, db.Get(key));
            }*/

            db.Close();

            string a = null;
            string b = "";
            string c = "false";
            string d = "{\"a\":111}";
            string e = "[{\"a\":111}]";

            var a1 = JsonConvert.DeserializeObject(a ?? "null");
            var b1 = JsonConvert.DeserializeObject(b);
            var c1 = JsonConvert.DeserializeObject(c);
            var d1 = JsonConvert.DeserializeObject(d);
            var e1 = JsonConvert.DeserializeObject(e);

            Console.WriteLine(JsonConvert.SerializeObject(a1));
            Console.WriteLine(JsonConvert.SerializeObject(b1));
            Console.WriteLine(JsonConvert.SerializeObject(c1));
            Console.WriteLine(JsonConvert.SerializeObject(d1));
            Console.WriteLine(JsonConvert.SerializeObject(e1));

            Console.ReadKey();
        }

        void CompressWithZstd()
        {
            ZstdCompressionWithEncryption.CompressFolderWithPassword(@"E:\temp\project_11\新工程091402", @"E:\temp\project_11\新工程091402\output.zstd", "123456");
            //ZstdCompressionWithEncryption
        }
    }
}
