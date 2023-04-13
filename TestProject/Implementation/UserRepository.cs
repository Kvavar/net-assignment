using Work.Database;
using Work.Interfaces;

namespace Work.Implementation
{
    // Use user cloning to prevent operations by reference
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
            
            if(!_db.Users.TryAdd(user.Id, user.Clone()))
                throw new ArgumentException($"User with Id {user.Id} already exists.");
        }

        public User Read(Guid key)
        {
            if (_db.Users.TryGetValue(key, out var user))
                return user.Clone();
            
            // return null could be an option here,
            // but I wouldn't like to propagate nullable User further in a scope of a test task   
            throw new KeyNotFoundException($"User with Id {key} does not exist."); 
        }

        // Pure Update method as requires by the interface,
        // although CreateOrUpdate could work better to reduce number of calls to DB
        // or for handling cases where Update called for non-existing User
        public void Update(User user)
        {
            ValidateUser(user);

            if (_db.Users.ContainsKey(user.Id))
            {
                _db.Users[user.Id] = user.Clone();
            }
            else
            {
                throw new KeyNotFoundException($"User with Id {user.Id} does not exist.");
            }
        }

        // Throws an exception on missing user to allow clients to customize their behaviour for this case.
        // Although do nothing on missing user may be an option,
        // since it leaves the DB in the desired state (no specified user stored) and simplifies the logic of the repo 
        public void Remove(Guid key)
        {
            if (_db.Users.ContainsKey(key))
            {
                _db.Users.Remove(key);
            }
            else
            {
                throw new KeyNotFoundException($"User with Id {key} does not exist.");
            }
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
