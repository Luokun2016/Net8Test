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
            var currentSegment = new StringBuilder();
            var bracketSegment = new StringBuilder();

            bool inBracket = false;

            foreach (var c in path)
            {
                switch (c)
                {
                    case '[':
                        // 开始收集索引段
                        AddSegmentIfNotEmpty(segments, currentSegment);
                        inBracket = true;
                        break;

                    case ']':
                        // 结束索引段
                        if (inBracket)
                        {
                            segments.Add($"[{bracketSegment}]");
                            bracketSegment.Clear();
                            inBracket = false;
                        }
                        break;

                    case '.':
                        // 分隔符处理
                        if (!inBracket)
                        {
                            AddSegmentIfNotEmpty(segments, currentSegment);
                        }
                        else
                        {
                            bracketSegment.Append(c);
                        }
                        break;

                    default:
                        // 收集字符
                        if (inBracket)
                        {
                            bracketSegment.Append(c);
                        }
                        else
                        {
                            currentSegment.Append(c);
                        }
                        break;
                }
            }

            // 处理剩余字符
            AddSegmentIfNotEmpty(segments, currentSegment);

            var result = segments.Select(ParseSegment).ToList();

            if (result.Count != segments.Count)
            {
                throw new InvalidDataException("Invalid path format");
            }

            return result;
        }

        private static void AddSegmentIfNotEmpty(List<string> segments, StringBuilder segmentBuilder)
        {
            if (segmentBuilder.Length > 0)
            {
                segments.Add(segmentBuilder.ToString());
                segmentBuilder.Clear();
            }
        }

        private static PathSegment ParseSegment(string segment)
        {
            if (segment.StartsWith('[') && segment.EndsWith(']'))
            {
                // 索引段
                var indices = segment[1..^1].Split(':').Select(int.Parse).ToArray();
                return new IndexSegment(indices);
            }
            else
            {
                // 字段段
                return new FieldSegment(segment);
            }
        }
    }
}
