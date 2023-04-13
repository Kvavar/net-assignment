using Work.Database;
using Work.Interfaces;

namespace Work.Implementation
{
    public class UserRepository : IRepository<User, Guid>
    {
        private readonly MockDatabase _db;

        public UserRepository(MockDatabase db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        public void Create(User user)
        {
            ValidateUser(user);
            
            if(!_db.Users.TryAdd(user.UserId, user))
                throw new InvalidOperationException($"User with Id {user.UserId} already exists.");
        }

        public User Read(Guid key)
        {
            if (_db.Users.TryGetValue(key, out var user))
                return user;
            
            // return null could be an option here,
            // but I wouldn't like to propagate nullable User further in a scope of a test task   
            throw new InvalidOperationException($"User with Id {key} does not exist."); 
        }

        // Pure Update method as requires by the interface,
        // although UpdateOrCreate could work better to reduce number of calls to DB
        public void Update(User user)
        {
            ValidateUser(user);

            if (_db.Users.ContainsKey(user.UserId))
                _db.Users[user.UserId] = user;
            
            throw new InvalidOperationException($"User with Id {user.UserId} does not exist.");
        }

        // Throws an exception on missing user to allow clients to customize their behaviour for this case.
        // Although do nothing on missing user may be an option,
        // since it leaves the DB in the desired state (no specified user stored) and simplifies the logic of the repo 
        public void Remove(User user)
        {
            ValidateUser(user);

            if (_db.Users.ContainsKey(user.UserId))
                _db.Users.Remove(user.UserId);
            
            throw new InvalidOperationException($"User with Id {user.UserId} does not exist.");
        }
        
        // User should not be null based on nullable annotation, but I would prefer to make sure to avoid unexpected NRE
        private static void ValidateUser(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }
        }
    }
}
