using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagParseUtility.NodeParse;
using TagParseUtility.Utilities;

namespace TagParseUtility.TagDefine
{
    /// <summary>
    /// 数组节点
    /// </summary>
    public class ArrayTypeNode : BaseTypeNode
    {
        /// <summary>
        /// 子节点定义
        /// </summary>
        private readonly string _baseType;

        /// <summary>
        /// 维度信息
        /// </summary>
        private List<ArrayDim> _arrayDims { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="name"></param>
        /// <param name="item"></param>
        public ArrayTypeNode(Guid channelId, string name, List<ArrayDim> arrayDims, string baseTypeName) : base (channelId)
        {
            NodeType = NodeType.Array;
            Name = name;
            _arrayDims = arrayDims;
            _baseType = baseTypeName;
        }

        /// <summary>
        /// 获取节点实际数据类型
        /// </summary>
        public BaseTypeNode BaseType => AllNodes.GetValueOrDefault(_baseType);

        public List<ArrayDim> ArrayDims => _arrayDims;

        public override bool IsVaildSegment(List<PathSegment> pathSegments)
        {
            if (pathSegments is null || pathSegments.Count == 0)
            {
                return true;
            }

            var segments = pathSegments[0];

            if (segments is IndexSegment indexSegment)
            {
                // 索引范围检查
                if (!TagUtility.ValidateIndices(ArrayDims, indexSegment.Indices))
                {
                    return false;
                }
                else
                {
                    if (pathSegments.Count > 1)
                    {
                        return BaseType.IsVaildSegment(pathSegments.GetRange(1, pathSegments.Count - 1));
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override IEnumerable<T> Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }
}
