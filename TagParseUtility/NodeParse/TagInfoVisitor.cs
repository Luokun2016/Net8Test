using System;
using System.Collections.Generic;
using System.Linq;
using TagParseUtility.TagDefine;
using TagParseUtility.Utilities;

namespace TagParseUtility.NodeParse
{
    public class TagInfoVisitor : IVisitor<TagInfoVisitor>
    {
        public string TagPath { get; set; }

        public string OrigonType { get; set; }

        public string FsuType { get; set; }

        public bool IsArray { get; set; }

        public IEnumerable<TagInfoVisitor> Visit(BasicTypeNode node)
        {
            return [];
        }

        public IEnumerable<TagInfoVisitor> Visit(ArrayTypeNode node)
        {
            var expends = TagUtility.GenerateCombinations(node.ArrayDims);

            var joinChar = string.Empty;
            if (node.BaseType.NodeType != NodeType.Array)
            {
                joinChar = ".";
            }

            var basicTypeNode = node.BaseType as BasicTypeNode;

            foreach (var indices in expends)
            {
                var subItems = node.BaseType.Accept(this);
                if (subItems.Any())
                {
                    foreach (var subItem in subItems)
                    {
                        subItem.TagPath = $"[{string.Join(":", indices)}]{joinChar}{subItem.TagPath}";
                        if (basicTypeNode != null)
                        {
                            subItem.OrigonType = basicTypeNode.OriginType;
                            subItem.FsuType = basicTypeNode.FSUType;
                        }
                        yield return subItem;
                    }
                }
                else
                {
                    var tagInfo = new TagInfoVisitor
                    {
                        TagPath = $"[{string.Join(":", indices)}]",
                    };

                    if (basicTypeNode != null)
                    {
                        tagInfo.OrigonType = basicTypeNode.OriginType;
                        tagInfo.FsuType = basicTypeNode.FSUType;
                    }

                    yield return tagInfo;
                }
            }
        }

        public IEnumerable<TagInfoVisitor> Visit(StructTypeNode node)
        {
            foreach (var item in node.GetItems())
            {
                var subItems = item.Value.Accept(this);
                var joinChar = string.Empty;
                if (item.Value.NodeType != NodeType.Array)
                {
                    joinChar = ".";
                }
                if (subItems.Any())
                {
                    foreach (var subItem in subItems)
                    {
                        subItem.TagPath = $"{item.Key}{joinChar}{subItem.TagPath}";
                        yield return subItem;
                    }
                }
                else
                {
                    var tagInfo = new TagInfoVisitor
                    {
                        TagPath = $"{item.Key}",
                    };

                    if (item.Value is BasicTypeNode basicTypeNode)
                    {
                        tagInfo.OrigonType = basicTypeNode.OriginType;
                        tagInfo.FsuType = basicTypeNode.FSUType;
                    }

                    yield return tagInfo;
                }
            }
        }

        public Func<TagInfoVisitor, int, TagInfoVisitor> GetArraySelector(string tagName) =>
            (tagInfo, _) =>
            {
                tagInfo.TagPath = $"{tagName}{tagInfo.TagPath}";
                return tagInfo;
            };

        public Func<TagInfoVisitor, int, TagInfoVisitor> GetStructSelector(string tagName) =>
            (tagInfo, _) =>
            {
                tagInfo.TagPath = $"{tagName}.{tagInfo.TagPath}";
                return tagInfo;
            };

        public IEnumerable<TagInfoVisitor> GetBasicTagInfo(string name, BasicTypeNode? basicNode)
        {
            return new List<TagInfoVisitor> { new TagInfoVisitor { TagPath = name, OrigonType = basicNode?.OriginType, FsuType = basicNode?.FSUType } };
        }
    }
}
