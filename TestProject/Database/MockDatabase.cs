namespace Work.Database
{
    public class MockDatabase
    {
        public Dictionary<Guid, User> Users { get; private set; } = new ();
    }
}
