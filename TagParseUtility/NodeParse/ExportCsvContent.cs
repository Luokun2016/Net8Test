using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagParseUtility.NodeParse
{
    public class ExportCsvContent
    {
        /// <summary>
        /// 索引
        /// </summary>
        public long Index { get; set; }

        public string Path { get; set; }

        public string OrigonType { get; set; }

        public string FsuType { get; set; }
    }
}
