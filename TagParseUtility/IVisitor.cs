using System;
using System.Collections.Generic;
using TagParseUtility.TagDefine;

namespace TagParseUtility
{
    public interface IVisitor<T>
    {
        /// <summary>
        /// 访问基本类型节点
        /// </summary>
        /// <param name="node">基本类型节点</param>
        IEnumerable<T> Visit(BasicTypeNode node);

        /// <summary>
        /// 访问数组类型节点
        /// </summary>
        /// <param name="node">数组类型节点</param>
        IEnumerable<T> Visit(ArrayTypeNode node);

        /// <summary>
        /// 访问结构体类型节点
        /// </summary>
        /// <param name="node">结构体类型节点</param>
        IEnumerable<T> Visit(StructTypeNode node);

        Func<T, int, T> GetArraySelector(string tagName);

        Func<T, int, T> GetStructSelector(string name);

        IEnumerable<T> GetBasicTagInfo(string name, BasicTypeNode? basicNode);
    }
}
