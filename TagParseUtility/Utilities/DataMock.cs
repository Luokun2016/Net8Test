using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagParseUtility.TagDefine;

namespace TagParseUtility.Utilities
{
    public  class DataMock
    {
        // Mock添加节点类型
        public static List<TagNode> MockNodes(Guid channelId, int driverId)
        {
            var result = new List<TagNode>();

            // 数据类型定义
            var a = TypeNodeCache.GetOrCreateTypeNode(new BasicTypeNode(channelId, "int", "int"));
            var b = TypeNodeCache.GetOrCreateTypeNode(new ArrayTypeNode(channelId, "array__0__2__of_int", new List<ArrayDim> { new ArrayDim { Start = 0, End = 2 } }, a.Name));
            var c = TypeNodeCache.GetOrCreateTypeNode(new ArrayTypeNode(channelId, "array__1__3__3__4_of_array__0__2__of_int", new List<ArrayDim> { new() { Start = 1, End = 3 }, new() { Start = 3, End = 4 } }, b.Name));
            var field1 = TypeNodeCache.GetOrCreateTypeNode(new BasicTypeNode(channelId, "bool", "bool", TypeConvertOption.GetTypeConvertByDriverId(driverId)));
            var field2 = TypeNodeCache.GetOrCreateTypeNode(new BasicTypeNode(channelId, "ulint", "ulint", TypeConvertOption.GetTypeConvertByDriverId(driverId)));

            var e = TypeNodeCache.GetOrCreateTypeNode(new StructTypeNode(channelId, "struct2", new Dictionary<string, string> { { "field1", field1.Name }, { "field2", field2.Name }, }));

            var d = TypeNodeCache.GetOrCreateTypeNode(new StructTypeNode(channelId, "struct1", new Dictionary<string, string> { { "a", a.Name }, { "b", b.Name }, { "c", c.Name }, { "d", e.Name }, }));

            var f = TypeNodeCache.GetOrCreateTypeNode(new ArrayTypeNode(channelId, "array__3__5__4__7_of_e", new List<ArrayDim> { new() { Start = 3, End = 5 }, new() { Start = 4, End = 7 } }, e.Name));

            result.Add(new TagNode { Name = "Tag1", BaseTypeName = a.Name });
            result.Add(new TagNode { Name = "Tag2", BaseTypeName = b.Name });
            result.Add(new TagNode { Name = "Tag3", BaseTypeName = c.Name });
            result.Add(new TagNode { Name = "Tag4", BaseTypeName = d.Name });
            result.Add(new TagNode { Name = "Tag5", BaseTypeName = e.Name });
            result.Add(new TagNode { Name = "Tag6", BaseTypeName = f.Name });

            return result;
        }
    }
}
