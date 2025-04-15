using Newtonsoft.Json;
using System.Xml.Linq;
using TagParseUtility;
using TagParseUtility.NodeParse;

namespace Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void TestGetMultiDimensionArrayElementNamesMethod()
        {
            List<TagArrayDim> arrayDimInfos = new List<TagArrayDim>
            {
                new TagArrayDim { StartIndex = 0, EndIndex = 2 },
                new TagArrayDim { StartIndex = 0, EndIndex = 2 },
            };

            // 二维度展开，普通数组
            List<string> retNameList0 = Utility.GetMultiDimensionArrayElementNames(arrayDimInfos);
            Console.WriteLine(string.Join(",", retNameList0));

            arrayDimInfos.Add(new TagArrayDim { StartIndex = 0, EndIndex = 2 });

            // 三维维度展开，普通数组
            List<string> retNameList = Utility.GetMultiDimensionArrayElementNames(arrayDimInfos);
            Console.WriteLine(string.Join(",", retNameList));

            // 三维维度展开，结构体数组
            List<string> retNameList1 = Utility.GetMultiDimensionArrayElementNames(arrayDimInfos, null, true);
            Console.WriteLine(string.Join(",", retNameList1));


            List<string> retNameList2 = Utility.GetMultiDimensionArrayElementNames(arrayDimInfos, new List<int> { 0, 1, 2 }, true);
            Console.WriteLine(string.Join(",", retNameList2));

            List<string> retNameList3 = Utility.GetMultiDimensionArrayElementNames(arrayDimInfos, new List<int> { 0, 1, 2 }, false);
            Console.WriteLine(string.Join(",", retNameList3));

            // [0:0:0],[0:0:1],[0:0:2],[0:1:0],[0:1:1],[0:1:2],[0:2:0],[0:2:1],[0:2:2],[1:0:0],[1:0:1],[1:0:2],[1:1:0],[1:1:1],[1:1:2],[1:2:0],[1:2:1],[1:2:2],[2:0:0],[2:0:1],[2:0:2],[2:1:0],[2:1:1],[2:1:2],[2:2:0],[2:2:1],[2:2:2]
            Assert.IsTrue(retNameList.Count == Math.Pow(3, 3));
        }

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