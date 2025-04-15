using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagParseUtility.TagDefine
{
    public static class TypeNodeCache
    {
        private static readonly ConcurrentDictionary<Guid, Dictionary<string, BaseTypeNode>> _cacheNode = new();

        public static Dictionary<string, BaseTypeNode> GetCacheNodeByKey(Guid channelId)
        {
            if (_cacheNode.TryGetValue(channelId, out var node))
            {
                return node;
            }

            // 如果没有找到，返回一个新的空字典
            var newNodeCache = new Dictionary<string, BaseTypeNode>();
            _cacheNode.TryAdd(channelId, newNodeCache);
            return newNodeCache;
        }

        /// <summary>
        /// 获取或创建数据类型节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static BaseTypeNode GetOrCreateTypeNode(BaseTypeNode node)
        {
            if (node.AllNodes.TryGetValue(node.Name, out BaseTypeNode? value))
            {
                return value;
            }
            node.AllNodes.Add(node.Name, node);
            return node;
        }
    }
}
