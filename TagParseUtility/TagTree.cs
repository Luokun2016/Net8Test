using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagParseUtility.NodeParse;
using TagParseUtility.TagDefine;
using TagParseUtility.Utilities;

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
            AllTagNodes = DataMock.MockNodes(_channelId, _driverId);
        }

        /// <summary>
        /// 验证路径是否合法
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsVaildPath(string path)
        {
            var pathSegments = TagUtility.Parse(path);

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
                        OrigonType = tagInfo.OriginType,
                        FsuType = tagInfo.FsuType
                    });
                }
            }

            return result;
        }
    }
}
