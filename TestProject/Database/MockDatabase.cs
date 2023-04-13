namespace Work.Database
{
    public class MockDatabase
    {
        public Dictionary<Guid, User> Users { get; private set; }

        public MockDatabase(Dictionary<Guid, User> users)
        {
            Users = users;
        }

        public MockDatabase(int seed)
        {
            Users = new Dictionary<Guid, User>();
            for (int i = 0; i < seed; i++)
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = $"User {i}",
                    Birthday = DateTime.Now.AddYears(-i)
                };
                Users.Add(user.Id, user);
            }
        }
    }
}
