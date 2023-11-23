using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using UsersAPI.Model;
using UsersAPI.Repository.Interfaces;
using UsersAPI.Util;

namespace UsersAPI.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly IMongoCollection<User> _users;

		public UserRepository(IOptions<DBSettings> mongoDB)
		{
			var mongoClient = new MongoClient(mongoDB.Value.ConnectionString);
			var mongoDatabase = mongoClient.GetDatabase(mongoDB.Value.DatabaseName);
			_users = mongoDatabase.GetCollection<User>(mongoDB.Value.UserCollectionName);


			var indexKeysDefinitionUserId = Builders<User>.IndexKeys.Ascending(u => u.Id);
			var indexModelUserId = new CreateIndexModel<User>(indexKeysDefinitionUserId);
			_users.Indexes.CreateOne(indexModelUserId);

			var indexKeysDefinitionUserName = Builders<User>.IndexKeys.Ascending(u => u.UserName);
			var indexModelUserName = new CreateIndexModel<User>(indexKeysDefinitionUserName);
			_users.Indexes.CreateOne(indexModelUserName);
		}

		public async Task Add(RegistrationRequest request)
		{
			string hashedPassword = HashingHelper.HashPassword(request.Password, out string salt);

			User user = new User
			{
				UserName = request.name,
				Email = request.Email,
				HashedPassword = hashedPassword,
				Salt = salt,
				Names = new List<Names>()
			};

			await _users.InsertOneAsync(user);
		}


		public async Task<bool> Authenticate(string email, string pwd)
		{
			User userMatch = await _users.Find(u => u.Email == email).FirstAsync();

			if (userMatch == null || userMatch.HashedPassword == null || userMatch.Salt == null) { return false; }

			return HashingHelper.Verify(pwd, userMatch.HashedPassword, userMatch.Salt);
		}


		public async Task Delete(ObjectId id)
		{
			await _users.DeleteOneAsync(user => user.Id == id);
		}

		public async Task<User> Get(ObjectId id)
		{
			return await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
		}

		public async Task<List<User>> GetAll()
		{
			return await _users.Find(user => true).ToListAsync();
		}

		public async Task<ObjectId> GetIdFromEmail(string email)
		{
			User user = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
			
			return user.Id;
		}
	}
}
