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

			var indexKeysDefinitionEmail = Builders<User>.IndexKeys.Ascending(u => u.Email);
			var indexModelEmail = new CreateIndexModel<User>(indexKeysDefinitionEmail);
			_users.Indexes.CreateOne(indexModelEmail);
		}

		public async Task AddAsync(RegistrationRequest request)
		{
			string hashedPassword = HashingHelper.HashPassword(request.Password, out string salt);

			User user = new User
			{
				UserName = request.Name,
				Email = request.Email,
				HashedPassword = hashedPassword,
				Salt = salt,
				Names = new List<string>()
			};

			await _users.InsertOneAsync(user);
		}


		public async Task<bool> AuthenticateAsync(string email, string pwd)
		{
			User userMatch = await _users.Find(u => u.Email == email).FirstAsync();

			if (userMatch == null || userMatch.HashedPassword == null || userMatch.Salt == null) { return false; }

			return HashingHelper.Verify(pwd, userMatch.HashedPassword, userMatch.Salt);
		}


		public async Task DeleteAsync(ObjectId id)
		{
			await _users.DeleteOneAsync(user => user.Id == id);
		}

		public async Task<User> GetAsync(ObjectId id)
		{
			return await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
		}

		public async Task<List<User>> GetAllAsync()
		{
			return await _users.Find(user => true).ToListAsync();
		}

		public async Task<User> GetByEmailAsync(string email)
		{	
			return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
		}

		public async Task AddNameToUserAsync(string name, string email)
		{
			var filter = Builders<User>.Filter.Eq(u => u.Email, email);
			var update = Builders<User>.Update.Push(u => u.Names,  name);

			var result = await _users.UpdateOneAsync(filter, update);
		}

		public async Task PatchPartnerLink(string name)
		{
			var filter = Builders<User>.Filter.Eq(u => u.UserName, name);
			var update = Builders<User>.Update.Set(u => u.Partner, name);

			await _users.UpdateOneAsync(filter, update);
		}
	}
}
