using MessagePack;
using Newtonsoft.Json.Linq;

namespace Net8Test.Utility
{
    public class DataEventRule
    {
        /// <summary>
        /// 条件表达式根节点
        /// </summary>
        public RuleGroup Rules { get; set; }

        /// <summary>
        /// 表达式编号
        /// </summary>
        public string ConditionExpr { get; set; }

        /// <summary>
        /// 表达式编号
        /// </summary>
        public string ConditionExprText { get; set; }

        /// <summary>
        /// 表达式编号
        /// </summary>
        public List<string> InputNames { get; set; }

        /// <summary>
        /// 受影响的事件id
        /// </summary>
        public List<string> EffectedVariableIds { get; set; }

        /// <summary>
        /// 变量属性
        /// </summary>
        public JObject VariableProperties { get; set; }
    }

    /// <summary>
    /// 条件表达式分组
    /// </summary>
    public class RuleGroup
    {
        /// <summary>
        /// 表达式编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 是否取反
        /// </summary>
        public bool Not { get; set; }

        /// <summary>
        /// 条件子集
        /// </summary>
        public List<Rule> Rules { get; set; }

        /// <summary>
        /// 条件分组之前的连接符
        /// </summary>
        public string Combinator { get; set; }
    }

    /// <summary>
    /// 条件表达式条件
    /// </summary>
    [MessagePackObject(true)]
    public partial class Rule
    {
        /// <summary>
        /// 表达式编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// 条件左边变量id
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 条件右边的变量值，只有当VariableType=Cosnt需设置
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 比较符
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 条件左边变量显示名
        /// </summary>
        public string VariableDisplayName { get; set; }

        /// <summary>
        /// 条件左边变量分组名称
        /// </summary>
        public string VariableGroupName { get; set; }

        /// <summary>
        /// 条件左边类型
        /// </summary>
        public RuleValueType VariableType { get; set; }

        /// <summary>
        /// 条件左值变量类型
        /// </summary>
        public string VariableDataType { get; set; }

        /// <summary>
        /// 虚拟变量配置
        /// </summary>
        public string VariableConfig { get; set; }

        /// <summary>
        /// 条件左值变量类型(systemVariable)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 条件左值变量路径
        /// </summary>
        public string VariablePath { get; set; }

        /// <summary>
        /// 条件左值变量属性
        /// </summary>
        public string VariableAttribute { get; set; }

        /// <summary>
        /// 条件左值变量按位索引
        /// </summary>
        public int? VariableBitIndex { get; set; }

        /// <summary>
        /// 条件右边的类型
        /// </summary>
        public RuleValueType ValueType { get; set; }

        /// <summary>
        /// 与ValueVariableId变量的DataType一致，当表达式右边是变量时才设置此字段.
        /// </summary>
        public string InputType { get; set; }

        /// <summary>
        /// 条件右边值变量id
        /// </summary>
        public string ValueVariableId { get; set; }

        /// <summary>
        /// 条件右边变量名称
        /// </summary>
        public string ValueVariableName { get; set; }

        /// <summary>
        /// 条件右边变量显示名
        /// </summary>
        public string ValueVariableDisplayName { get; set; }

        /// <summary>
        /// 条件右边变量分组名称
        /// </summary>
        public string ValueVariableGroupName { get; set; }

        /// <summary>
        /// 值是虚拟变量时的配置
        /// </summary>
        public string ValueVariableConfig { get; set; }

        /// <summary>
        /// 条件右边变量类型(systemVariable)
        /// </summary>
        public string ValueVariableType { get; set; }

        /// <summary>
        /// 条件右边变量路径(systemVariable)
        /// </summary>
        public string ValueVariablePath { get; set; }

        /// <summary>
        /// 条件右边变量路径(systemVariable)
        /// </summary>
        public string InputAttribute { get; set; }

        /// <summary>
        /// 条件左值变量按位索引
        /// </summary>
        public int? ValueVariableBitIndex { get; set; }
    }

    /// <summary>
    /// 条件与方法体的参数类型
    /// </summary>
    public enum RuleValueType
    {
        /// <summary>
        /// Warning
        /// </summary>
        Constant = 0,

        /// <summary>
        /// Minor
        /// </summary>
        Variable = 1,

        /// <summary>
        /// Major
        /// </summary>
        Input = 2,

        /// <summary>
        /// Critical
        /// </summary>
        Response = 3,
    }

    public class RuleComparer : IEqualityComparer<Rule>
    {
        public bool Equals(Rule? x, Rule? y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.Value == y.Value && x.Id == x.Id;
        }

        public int GetHashCode(Rule obj)
        {
            return (obj.Value, obj.Id).GetHashCode();
        }
    }
}
