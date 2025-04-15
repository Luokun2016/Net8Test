using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagParseUtility.TagDefine
{
    public class TagNode
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string BaseTypeName { get; set; }

        public string Description { get; set; }

        public Guid? ParentId { get; set; }
    }
}
