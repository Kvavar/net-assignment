using FluentAssertions;
using Work.Database;
using Work.Implementation;
using Work.Interfaces;

namespace Tests;

public class Tests
{
    private IRepository<User, Guid> _repository;

    [SetUp]
    public void Setup()
    {
        var db = new MockDatabase();
        _repository = new UserRepository(db);
    }

    // Ideally we want to test Create and Read operations separately (as unit testing assumes),
    // but I would like to avoid manipulating with underlying Dictionary directly,
    // so the tests do not depend on database implementation.
    // Another option is to add Exists method to the repository,
    // but although it can be useful in some scenarios to avoid loading entire entities
    // I think it is unnecessary here 
    [Test]
    public void TestCreateReadSuccess()
    {
        var expected = CreateUser();
        
        var actual = _repository.Read(expected.Id);

        actual.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public void TestUpdateSuccess()
    {
        var expected = CreateUser();
        expected.Name = "Test user updated";

        _repository.Update(expected);

        var actual = _repository.Read(expected.Id);
        actual.Should().BeEquivalentTo(expected);
    }
    
    // One database implementation specific test here to ensure the repository does not perform operations by reference
    [Test]
    public void TestUpdateByRef()
    {
        var user = CreateUser();
        var expected = user.Clone();
        user.Name = "Test user updated";

        var actual = _repository.Read(user.Id);
        
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public void TestDeleteSuccess()
    {
        var expected = CreateUser();

        _repository.Remove(expected.Id);
        
        var operation = () => { _repository.Read(expected.Id); };

        operation.Should().ThrowExactly<KeyNotFoundException>();
    }
    
    [Test]
    public void TestCreateFail()
    {
        var expected = CreateUser();

        var operation = () => { _repository.Create(expected); };

        operation.Should().ThrowExactly<ArgumentException>();
    }
    
    [Test]
    public void TestCreateValidationFail()
    {
        var operation = () => { _repository.Create(null); };

        operation.Should().ThrowExactly<ArgumentNullException>();
    }
    
    [Test]
    public void TestReadFail()
    {
        var operation = () => { _repository.Read(Guid.NewGuid()); };

        operation.Should().ThrowExactly<KeyNotFoundException>();
    }
    
    [Test]
    public void TestUpdateFail()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test user",
            Birthday = DateTime.UtcNow
        };
        
        var operation = () => { _repository.Update(user); };

        operation.Should().ThrowExactly<KeyNotFoundException>();
    }
    
    [Test]
    public void TestDeleteFail()
    {
        var operation = () => { _repository.Remove(Guid.NewGuid()); };

        operation.Should().ThrowExactly<KeyNotFoundException>();
    }

    private User CreateUser()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Test user",
            Birthday = DateTime.UtcNow
        };

        _repository.Create(user);
        
        return user;
    }
}