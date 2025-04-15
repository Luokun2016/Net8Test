using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Linq;
using TagParseUtility.NodeParse;
using TagParseUtility.TagDefine;

namespace TagParseUtility
{
    public class TagTree
    {
        private readonly int _driverId;

        private readonly Guid _channelId;

        /// <summary>
        /// 所有标签节点
        /// </summary>
        public readonly List<TagNode> AllTagNodes;

        /// <summary>
        /// 所有数据类型节点
        /// </summary>
        private Dictionary<string, BaseTypeNode> AllTypeNodes => TypeNodeCache.GetCacheNodeByKey(_channelId);

        public TagTree(Guid channelId, int driverId)
        {
            _driverId = driverId;
            _channelId = channelId;
            AllTagNodes = MockNodes();
        }

        // Mock添加节点类型
        private List<TagNode> MockNodes()
        {
            var result = new List<TagNode>();

            // 数据类型定义
            var a = TypeNodeCache.GetOrCreateTypeNode(new BasicTypeNode(_channelId, "int", "int"));
            var b = TypeNodeCache.GetOrCreateTypeNode(new ArrayTypeNode(_channelId, "array__0__2__of_int", new List<ArrayDim> { new ArrayDim { Start = 0, End = 2 } }, a.Name));
            var c = TypeNodeCache.GetOrCreateTypeNode(new ArrayTypeNode(_channelId, "array__1__3__3__4_of_array__0__2__of_int", new List<ArrayDim> { new() { Start = 1, End = 3 }, new() { Start = 3, End = 4 } }, b.Name));
            var field1 = TypeNodeCache.GetOrCreateTypeNode(new BasicTypeNode(_channelId, "bool", "bool", TypeConvertOption.GetTypeConvertByDriverId(_driverId)));
            var field2 = TypeNodeCache.GetOrCreateTypeNode(new BasicTypeNode(_channelId, "ulint", "ulint", TypeConvertOption.GetTypeConvertByDriverId(_driverId)));

            var e = TypeNodeCache.GetOrCreateTypeNode(new StructTypeNode(_channelId, "struct2", new Dictionary<string, string> { { "field1", field1.Name }, { "field2", field2.Name }, }));

            var d = TypeNodeCache.GetOrCreateTypeNode(new StructTypeNode(_channelId, "struct1", new Dictionary<string, string> { { "a", a.Name }, { "b", b.Name }, { "c", c.Name }, { "d", e.Name }, }));

            var f = TypeNodeCache.GetOrCreateTypeNode(new ArrayTypeNode(_channelId, "array__3__5__4__7_of_e", new List<ArrayDim> { new() { Start = 3, End = 5 }, new() { Start = 4, End = 7 } }, e.Name));

            result.Add(new TagNode { Name = "Tag1", BaseTypeName = a.Name });
            result.Add(new TagNode { Name = "Tag2", BaseTypeName = b.Name });
            result.Add(new TagNode { Name = "Tag3", BaseTypeName = c.Name });
            result.Add(new TagNode { Name = "Tag4", BaseTypeName = d.Name });
            result.Add(new TagNode { Name = "Tag5", BaseTypeName = e.Name });
            result.Add(new TagNode { Name = "Tag6", BaseTypeName = f.Name });

            return result;
        }

        /// <summary>
        /// 验证路径是否合法
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsVaildPath(string path)
        {
            var pathSegments = Parse(path);

            if (pathSegments.Count == 0)
            {
                return false;
            }

            if (pathSegments[0] is not FieldSegment tagField)
            {
                return false;
            }

            var tagNode = AllTagNodes.Find(p => p.Name == tagField.FieldName);
            if (tagNode is null)
            {
                return false;
            }

            if (pathSegments.Count > 1)
            {
                if (AllTypeNodes.TryGetValue(tagNode.BaseTypeName, out var nodeType))
                {
                    return nodeType.IsVaildSegment(pathSegments.GetRange(1, pathSegments.Count - 1));
                }

                return false;
            }

            return true;
        }

        public IEnumerable<string> SearchTagPath(TagSearchArg searchArg)
        {
            var result = new List<string>();
            foreach (var tag in AllTagNodes)
            {
                result.AddRange(SearchTagNames(tag, searchArg));
            }

            return result;
        }

        private IEnumerable<string> SearchTagNames(TagNode tag, TagSearchArg searchArg)
        {
            foreach (var tagInfo in GetTagInfos(tag, new TagInfoVisitor()))
            {
                var match = true;

                if (!string.IsNullOrEmpty(searchArg.FsuType) && !string.Equals(tagInfo.FsuType, searchArg.FsuType))
                {
                    match = false;
                }

                if (match && !string.IsNullOrEmpty(searchArg.TagName) && !tagInfo.TagPath.Contains(searchArg.TagName))
                {
                    match = false;
                }

                if (match)
                {
                    yield return tagInfo.TagPath;
                }
            }
        }

        public static List<PathSegment> Parse(string path)
        {
            var segments = new List<string>();
            var buffer = new List<char>();
            var inBracket = false;
            var bracketBuffer = new List<char>();

            foreach (var c in path)
            {
                if (c == '[')
                {
                    // 遇到左括号，开始收集索引段
                    if (buffer.Count > 0)
                    {
                        segments.Add(new string(buffer.ToArray()));
                        buffer.Clear();
                    }
                    inBracket = true;
                    bracketBuffer.Clear();
                }
                else if (c == ']')
                {
                    // 遇到右括号，结束索引段并处理
                    if (inBracket)
                    {
                        // 将冒号替换为逗号，并包装成 [1,2,3] 格式
                        segments.Add($"[{new string(bracketBuffer.ToArray())}]");
                        bracketBuffer.Clear();
                        inBracket = false;
                    }
                }
                else if (c == '.' && !inBracket)
                {
                    // 非索引段的分隔符
                    if (buffer.Count > 0)
                    {
                        segments.Add(new string(buffer.ToArray()));
                        buffer.Clear();
                    }
                }
                else
                {
                    // 收集字符到缓冲区
                    if (inBracket) bracketBuffer.Add(c);
                    else buffer.Add(c);
                }
            }

            // 处理剩余字符
            if (buffer.Count > 0) segments.Add(new string(buffer.ToArray()));

            var result = new List<PathSegment>();
            foreach (var seg in segments)
            {
                if (seg.StartsWith('[') && seg.EndsWith(']'))
                {
                    // 索引段
                    var indices = seg[1..^1].Split(':').Select(int.Parse).ToArray();
                    result.Add(new IndexSegment(indices));
                }
                else
                {
                    // 字段段
                    result.Add(new FieldSegment(seg));
                }
            }

            if (result.Count != segments.Count)
            {
                throw new InvalidDataException("Invalid path format");
            }

            return result;
        }

        /// <summary>
        /// 展开子节点
        /// </summary>
        /// <param name="tagNode"></param>
        /// <returns></returns>
        public IEnumerable<T> GetTagInfos<T>(TagNode tagNode, IVisitor<T> visitor)
        {
            if (AllTypeNodes.TryGetValue(tagNode.BaseTypeName, out var node))
            {
                if (node.NodeType == NodeType.Array)
                {
                    return node.Accept(visitor).Select(visitor.GetArraySelector(tagNode.Name));
                }
                else if (node.NodeType == NodeType.Struct)
                {
                    return node.Accept(visitor).Select(visitor.GetStructSelector(tagNode.Name));
                }
                else if (node.NodeType == NodeType.Basic)
                {
                    var basicNode = node as BasicTypeNode;
                    return visitor.GetBasicTagInfo(tagNode.Name, basicNode);
                }
            }

            return [];
        }

        public IEnumerable<string> GetTagPaths(TagNode tagNode)
        {
            var visitor = new TagInfoVisitor();
            return GetTagInfos(tagNode, visitor).Select(p => p.TagPath);
        }

        /// <summary>
        /// 输出 CSV 内容
        /// </summary>
        /// <param name="convertFunc"></param>
        /// <returns></returns>
        public IEnumerable<ExportCsvContent> ExportCSVContent()
        {
            var result = new List<ExportCsvContent>();
            var index = 1;
            foreach (var tag in AllTagNodes)
            {
                foreach (var tagInfo in GetTagInfos(tag, new TagInfoVisitor()))
                {
                    var convertFunc = TypeConvertOption.GetExportConvertByDriverId<TagInfoVisitor>(_driverId);
                    if (convertFunc != null)
                    {
                        var content = convertFunc.Invoke(tagInfo);
                        content.Index = index++;
                        result.Add(content);
                        continue;
                    }

                    // 如果没有转换函数，则使用默认的转换逻辑
                    result.Add(new ExportCsvContent
                    {
                        Index = index++,
                        Path = tagInfo.TagPath,
                        OrigonType = tagInfo.OrigonType,
                        FsuType = tagInfo.FsuType
                    });
                }
            }

            return result;
        }
    }
}
