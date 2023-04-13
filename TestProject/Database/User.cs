namespace Work.Database;

public class User
{
    public Guid Id { get; init; }

    public string Name { get; set; }

    public DateTime Birthday { get; set; }


    public User Clone()
    {
        return (User)MemberwiseClone();
    }
}