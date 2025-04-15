using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagParseUtility.NodeParse;

namespace TagParseUtility.TagDefine
{
    /// <summary>
    /// 标签节点定义
    /// </summary>
    public abstract class BaseTypeNode
    {
        protected BaseTypeNode(Guid channelId)
        {
            ChannleId = channelId;
        }

        /// <summary>
        /// 所有节点
        /// </summary>
        public Dictionary<string, BaseTypeNode> AllNodes => TypeNodeCache.GetCacheNodeByKey(ChannleId);

        /// <summary>
        /// 节点名称(唯一)
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 信道ID
        /// </summary>
        private Guid ChannleId { get;}

        /// <summary>
        /// 节点类型
        /// </summary>
        public NodeType NodeType { get; protected set; }

        /// <summary>
        /// 访问者模式
        /// </summary>
        /// <param name="visitor"></param>
        public abstract IEnumerable<T> Accept<T>(IVisitor<T> visitor);

        /// <summary>
        /// 是否合法路径
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public abstract bool IsVaildSegment(List<PathSegment> pathSegments);

        /// <summary>
        /// GetHashCode
        /// </summary>
        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return 0;
            }
            else
            {
                return HashCode.Combine(Name, NodeType);
            }
        }
    }
}
