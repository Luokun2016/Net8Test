namespace TagParseUtility.NodeParse
{
    public abstract class PathSegment { }

    // 结构体字段段（如 `.a`）
    public class FieldSegment : PathSegment
    {
        public string FieldName { get; }
        public FieldSegment(string name) => FieldName = name;
    }

    // 数组索引段（如 `[5:9]`）
    public class IndexSegment : PathSegment
    {
        public int[] Indices { get; } // 多维索引，如 [i,j,k]
        public IndexSegment(params int[] indices) => Indices = indices;
    }
}
