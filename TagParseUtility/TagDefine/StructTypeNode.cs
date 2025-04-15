using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using TagParseUtility.NodeParse;

namespace TagParseUtility.TagDefine
{
    /// <summary>
    /// 结构体类型
    /// </summary>
    public class StructTypeNode : BaseTypeNode
    {
        /// <summary>
        /// 子节点定义(子节点名称， 子节点类型名称)
        /// </summary>
        private readonly Dictionary<string, string> _items = new();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="name"></param>
        /// <param name="items"></param>
        public StructTypeNode(Guid channelId, string name, Dictionary<string, string> items) : base (channelId)
        {
            NodeType = NodeType.Struct;
            Name = name;
            _items = items;
        }

        public override IEnumerable<T> Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);

        /// <summary>
        /// 获取子节点
        /// </summary>
        public Dictionary<string, BaseTypeNode> GetItems()
        {
            var result = new Dictionary<string, BaseTypeNode>();
            foreach (var item in _items)
            {
                result.Add(item.Key, AllNodes[item.Value]);
            }

            return result;
        }

        public override bool IsVaildSegment(List<PathSegment> pathSegments)
        {
            if (pathSegments is null || pathSegments.Count == 0)
            {
                return true;
            }

            var segments = pathSegments[0];
            if (segments is FieldSegment fieldSegment)
            {
                if (_items.TryGetValue(fieldSegment.FieldName, out var itemType) && AllNodes.TryGetValue(itemType, out var itemBaseType))
                {
                    if (pathSegments.Count > 1)
                    {
                        return itemBaseType.IsVaildSegment(pathSegments.GetRange(1, pathSegments.Count - 1));
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
