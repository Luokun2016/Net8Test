using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagParseUtility.NodeParse;
using TagParseUtility.TagDefine;

namespace TagParseUtility.Utilities
{
    public static class TagUtility
    {
        /// <summary>
        /// 生成所有可能的组合
        /// </summary>
        /// <param name="arrayDims"></param>
        /// <returns></returns>
        public static IEnumerable<int[]> GenerateCombinations(List<ArrayDim> arrayDims)
        {
            int[] current = new int[arrayDims.Count];

            // 初始化每个维度为起始值
            for (int i = 0; i < arrayDims.Count; i++)
            {
                current[i] = arrayDims[i].Start;
            }

            bool hasNext = true;
            while (hasNext)
            {
                // 复制当前组合到结果列表
                yield return (int[])current.Clone();

                int index = arrayDims.Count - 1;
                while (index >= 0)
                {
                    if (current[index] < arrayDims[index].End)
                    {
                        current[index]++;
                        break;
                    }
                    else
                    {
                        // 重置当前维度为起始值
                        current[index] = arrayDims[index].Start;
                        index--;
                    }
                }

                // 判断是否还有下一个组合
                hasNext = index >= 0;
            }
        }

        /// <summary>
        /// 验证索引是否在维度范围内
        /// </summary>
        /// <param name="dimensions"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public static bool ValidateIndices(List<ArrayDim> dimensions, int[] indices)
        {
            if (indices.Length != dimensions.Count) return false;
            for (int i = 0; i < indices.Length; i++)
            {
                if (indices[i] < dimensions[i].Start || indices[i] > dimensions[i].End)
                    return false;
            }
            return true;
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
    }
}
