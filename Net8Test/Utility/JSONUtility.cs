using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Net8Test.Utility
{
    public class JSONUtility
    {
        /// <summary>
        /// 小驼峰 格式化编译内容 忽略Null值
        /// </summary>
        /// <param name="content">编译内容</param>
        /// <returns>编译内容</returns>
        public static string FormatBuildContentByCamelIgnoreNullAndIndented(object content)
        {
            JsonSerializerSettings jsetting = BuildBasicJSetting(true);

            return JsonConvert.SerializeObject(content, Formatting.Indented, jsetting);
        }

        private static JsonSerializerSettings BuildBasicJSetting(bool ignoreNullValue = false)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings()
            {
                // 驼峰命名
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            if (ignoreNullValue)
            {
                jsetting.NullValueHandling = NullValueHandling.Ignore;
            }

            jsetting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // 允许dto之间相互循环引用
            IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffff"
            };

            jsetting.Converters.Add(new StringEnumConverter()); // 枚举值作为字符串下发
            jsetting.Converters.Add(isoDateTimeConverter); // 运行时处理的文件，时间格式统一为 yyyy-MM-ddTHH:mm:ss.fffffff
            return jsetting;
        }
    }
}
