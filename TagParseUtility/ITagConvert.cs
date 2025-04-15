using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagParseUtility.NodeParse;
using TagParseUtility.TagDefine;

namespace TagParseUtility
{
    public interface ITagConvert
    {
        /// <summary>
        /// 转换为 FSU 类型
        /// </summary>
        /// <param name="basicType">标签路径</param>
        /// <returns>转换后的 FSU 类型</returns>
        string ConvertToFsuType(BasicTypeNode basicType);

        int ConvertToWidth(BasicTypeNode basicType);

        int ConvertStringLength(BasicTypeNode basicType);
    }
}
