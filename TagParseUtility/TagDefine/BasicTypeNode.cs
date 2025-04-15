using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagParseUtility.NodeParse;

namespace TagParseUtility.TagDefine
{
    public class BasicTypeNode : BaseTypeNode
    {
        private readonly string _originType;

        private string _fsuType;

        /// <summary>
        /// 字符串长度
        /// </summary>
        private Dictionary<string, object> _properties;

        public BasicTypeNode(Guid channelId, string name, string originType) : base (channelId)
        {
            NodeType = NodeType.Basic;
            _originType = originType;
            _fsuType = originType;
            Name = name;
        }

        public BasicTypeNode(Guid channelId, string name, string originType, Func<string, string> convertFunc) : base(channelId)
        {
            NodeType = NodeType.Basic;
            _originType = originType;
            Name = name;

            ConvertFsuType(convertFunc);
        }

        public string OriginType => _originType;

        public string ConvertFsuType(Func<string, string> convertFunc)
        {
            if (convertFunc != null)
            {
                _fsuType = convertFunc.Invoke(_originType);
            }
            else
            {
                _fsuType = _originType;
            }

            return _fsuType;
        }

        public string FSUType => _fsuType;

        public override bool IsVaildSegment(List<PathSegment> pathSegments)
        {
            return pathSegments is null || pathSegments.Count == 0;
        }

        public override IEnumerable<T> Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
