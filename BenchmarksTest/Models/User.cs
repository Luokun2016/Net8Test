namespace BenchmarksTest.Models
{
    public class User
    {
        public User() { }

        public User(Guid guid, string v1, string v2, string v3, string v4, string v5)
        {
            Id = guid;
            FirstName = v1;
            LastName = v2;
            FullName = v3;
            UserName = v4;
            Email = v5;
        }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }
    }
}
