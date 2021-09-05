using System.Threading.Tasks;
using Identity.API.Models;
using MongoDB.Driver;

namespace Identity.API.Repository
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(IMongoCollection<User> userCollection) {
            _collection = userCollection;
        }

        public async Task<User?> FindUserFromGoogleId(string googleId) {
            var filter = Builders<User>.Filter.Eq(o => o.GoogleId, googleId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateNewUser(User user) {
            await _collection.InsertOneAsync(user);
        }
    }
}