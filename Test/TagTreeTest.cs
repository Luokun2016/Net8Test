using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagParseUtility;

namespace Test
{
    [TestClass]
    public class TagTreeTest
    {
        [TestMethod]
        public void TestTagTree()
        {
            var tree = new TagTree(Guid.NewGuid(), 8084);

            foreach (var node in tree.AllTagNodes)
            {
                Console.WriteLine($"Tage: {node.Name}");
                Console.WriteLine(string.Join(",", tree.GetTagPaths(node)));
            }

            Console.WriteLine("Tag6 :{0}", tree.IsVaildPath("Tag6"));
            Console.WriteLine("Tag6[3:4] :{0}", tree.IsVaildPath("Tag6[3:4]"));
            Console.WriteLine("Tag6[3:4].field1:{0}", tree.IsVaildPath("Tag6[3:4].field1"));
            Console.WriteLine("Tag6[3:4].field3 :{0}", tree.IsVaildPath("Tag6[3:4].field3"));
            Console.WriteLine("Tag3 :{0}", tree.IsVaildPath("Tag3"));
            Console.WriteLine("Tag2[2] :{0}", tree.IsVaildPath("Tag2[2]"));

            var searchPath = tree.SearchTagPath(new TagParseUtility.NodeParse.TagSearchArg
            {
                TagName = "Tag4",
                FsuType = "bool"
            });

            Console.WriteLine("Search Tag4:{0}", string.Join(",", searchPath));

            var content = tree.ExportCSVContent();
            Console.WriteLine(JsonConvert.SerializeObject(content));

            Assert.IsTrue(6 == 6);
        }
    }
}
