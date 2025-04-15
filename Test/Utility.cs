using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Utility
    {
        /// <summary>
        /// 多维度展开
        /// </summary>
        /// <param name="arrayDimInfos"></param>
        /// <returns></returns>
        public static List<string> CreateMultiDimensionArrayElementNames(List<TagArrayDim> arrayDimInfos)
        {
            List<string> retNameList = new List<string>();

            int arrDimLength = arrayDimInfos.Count;

            if (arrDimLength == 1)
            {
                for (int i = arrayDimInfos[0].StartIndex; i <= arrayDimInfos[0].EndIndex; i++)
                {
                    retNameList.Add($"[{i}]");
                }
            }
            else
            {
                for (int i = arrDimLength - 1; i >= 0; i--)
                {
                    string[] tempNameList = new string[retNameList.Count];
                    retNameList.CopyTo(tempNameList);
                    retNameList.Clear();
                    for (int j = arrayDimInfos[i].StartIndex; j <= arrayDimInfos[i].EndIndex; j++)
                    {
                        if (i == arrDimLength - 1)
                        {
                            retNameList.Add($"{j}]");
                        }
                        else if (i > 0)
                        {
                            foreach (string tempEleName in tempNameList)
                            {
                                retNameList.Add($"{j}:{tempEleName}");
                            }
                        }
                        else
                        {
                            foreach (string tempEleName in tempNameList)
                            {
                                retNameList.Add($"[{j}:{tempEleName}");
                            }
                        }
                    }
                }
            }

            return retNameList;
        }

        /// <summary>
        /// 获取多维数组/嵌套数组名称集合
        /// </summary>
        /// <param name="arrayDimInfos">多维数组信息</param>
        /// <param name="arrayDimensionals">嵌套数组每级的数组维度</param>
        /// <param name="isCustomType">是否多维数组结构体</param>
        /// <returns>名称集合</returns>
        public static List<string> GetMultiDimensionArrayElementNames(List<TagArrayDim> arrayDimInfos, List<int> arrayDimensionals = null, bool isCustomType = false)
        {
            if (arrayDimensionals == null)
            {
                return CreateMultiDimensionArrayElementNames(arrayDimInfos);
            }
            else
            {
                List<string> retNameList = new List<string>();
                int skip = 0;
                List<List<string>> dimensionalRetNameList = new List<List<string>>();
                for (int level = 0; level <= arrayDimensionals.Count - 1; level++)
                {
                    // 每级维度信息
                    var dimensional = arrayDimensionals[level];
                    if (level == arrayDimensionals.Count - 1 && !isCustomType)
                    {
                        dimensional = dimensional - 1;
                    }

                    var curreentLevelArrayDims = arrayDimInfos.Skip(skip).Take(dimensional).ToList();
                    if (curreentLevelArrayDims.Any())
                    {
                        dimensionalRetNameList.Add(CreateMultiDimensionArrayElementNames(curreentLevelArrayDims));
                    }

                    skip += dimensional;
                }

                // 生成数组元素名
                for (int i = dimensionalRetNameList.Count - 1; i >= 0; i--)
                {
                    string[] tempNameList = new string[retNameList.Count];
                    retNameList.CopyTo(tempNameList);
                    retNameList.Clear();
                    for (int j = 0; j <= dimensionalRetNameList[i].Count - 1; j++)
                    {
                        if (i == dimensionalRetNameList.Count - 1)
                        {
                            retNameList.Add(dimensionalRetNameList[i][j]);
                        }
                        else if (i > 0)
                        {
                            foreach (string tempEleName in tempNameList)
                            {
                                retNameList.Add($"{dimensionalRetNameList[i][j]}{tempEleName}");
                            }
                        }
                        else
                        {
                            foreach (string tempEleName in tempNameList)
                            {
                                retNameList.Add($"{dimensionalRetNameList[i][j]}{tempEleName}");
                            }
                        }
                    }
                }

                return retNameList;
            }
        }
    }
}
