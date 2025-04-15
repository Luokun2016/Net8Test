using System.Text.Json.Serialization;

namespace BenchmarksTest.Models
{
    [JsonSerializable(typeof(List<User>))]
    public partial class UserGenerationContext : JsonSerializerContext
    {
    }
}
