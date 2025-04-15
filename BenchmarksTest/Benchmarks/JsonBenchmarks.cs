using BenchmarkDotNet.Attributes;
using BenchmarksTest.Models;
using Bogus;
using System.Text.Json;

namespace BenchmarksTest.Benchmarks
{
    // [DotTraceDiagnoser]
    [MemoryDiagnoser]
    // [KeepBenchmarkFiles]
    public class JsonBenchmarks
    {
        [Benchmark]
        public void NewtonsoftSerializeBigData() =>
            _ = Newtonsoft.Json.JsonConvert.SerializeObject(testUsers);

        [Benchmark]
        public void MicrosoftSerializeBigData() =>
            _ = System.Text.Json.JsonSerializer.Serialize(testUsers);

        [Benchmark]
        public void NewtonsoftDeserializeBigData() =>
            _ = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(serializedTestUsers);

        [Benchmark]
        public void MicrosoftDeserializeBigData()
        {

            _ = System.Text.Json.JsonSerializer.Deserialize<List<User>>(serializedTestUsers, options);
        }

        [Params(100, 1000, 5000, 10000)]
        public int Count { get; set; }

        private List<User> testUsers = [];

        private string serializedTestUsers = string.Empty;

        private readonly JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true,
            TypeInfoResolver = UserGenerationContext.Default,
        };

        [GlobalSetup]
        public void GlobalSetup()
        {
            var faker = new Faker<User>().CustomInstantiator(
                f =>
                    new User(
                    Guid.NewGuid(),
                    f.Name.FirstName(),
                    f.Name.LastName(),
                    f.Name.FullName(),
                    f.Internet.UserName(f.Name.FirstName(), f.Name.LastName()),
                    f.Internet.Email(f.Name.FirstName(), f.Name.LastName())
                    )
            );

            testUsers = faker.Generate(Count);
            serializedTestUsers = System.Text.Json.JsonSerializer.Serialize(testUsers);
        }
    }
}
