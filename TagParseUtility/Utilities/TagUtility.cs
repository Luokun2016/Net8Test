using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
