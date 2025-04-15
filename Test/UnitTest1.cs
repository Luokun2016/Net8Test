using Newtonsoft.Json;
using TagParseUtility;

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
    }
}