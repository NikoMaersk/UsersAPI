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
			User userMatch = await _users.Find(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)).FirstAsync();

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
			return await _users.Find(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();
		}

		public async Task AddNameToUserAsync(string name, string email)
		{
			string formattedName = char.ToUpper(name[0]) + name.Substring(1).ToLower();

			var filter = Builders<User>.Filter.Eq(u => u.Email, email);
			var update = Builders<User>.Update.Push(u => u.Names,  formattedName);

			var result = await _users.UpdateOneAsync(filter, update);
		}

		public async Task PatchPartnerLinkAsync(string email, string linkMail)
		{
			var filter = Builders<User>.Filter.Eq(u => u.Email, email);
			var update = Builders<User>.Update.Set(u => u.Partner, linkMail);

			await _users.UpdateOneAsync(filter, update);
		}

		public async Task<bool> CheckIfUserExistsAsync(string email)
		{
			return await _users
				.Find(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
				.AnyAsync();
		}

		public async Task<bool> IsNameStoredAsync(string email, string name)
		{
			User user = await _users.Find(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();

            if (user != null)
            {
				return user.Names?.Any(n => n.Equals(name, StringComparison.OrdinalIgnoreCase)) ?? false;
			}

			return false;
		}

		public async Task<bool> ClearNamesListAsync(string email)
		{
			var filter = Builders<User>.Filter.Eq(u => u.Email, email);
			var update = Builders<User>.Update.Set(u => u.Names, new List<string>());

			var updateResult = await _users.UpdateOneAsync(filter, update);

			return updateResult?.ModifiedCount > 0;
		}

		public async Task<bool> ClearNameFromListAsync(string email, List<string> namesToRemove)
		{
			User user = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

			if (user == null)
			{
				return false;
			}

			var lowerCaseNames = namesToRemove.Select(n => n.ToLowerInvariant()).ToList();
			var updatedNames = user.Names.Where(name => !lowerCaseNames.Contains(name.ToLowerInvariant())).ToList();

			var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);
			var update = Builders<User>.Update.Set(u => u.Names, updatedNames);

			var updateResult = await _users.UpdateOneAsync(filter, update);

			return updateResult?.ModifiedCount > 0;
		}

		public async Task<bool> PatchEmailAsync(string oldEmail, string newEmail)
		{
			var filter = Builders<User>.Filter.Eq(u => u.Email, oldEmail);
			var update = Builders<User>.Update.Set(u => u.Email, newEmail);

			var updateResult = await _users.UpdateOneAsync(filter, update);

			return updateResult?.ModifiedCount > 0;
		}

		public async Task<bool> PatchPasswordAsync(string email, string password)
		{
			var filter = Builders<User>.Filter.Eq(u => u.Email, email);
			string hashedPassword = HashingHelper.HashPassword(password, out string salt);
			var update = Builders<User>.Update.Set(u => u.HashedPassword, hashedPassword).Set(u => u.Salt, salt);

			var updateResult = await _users.UpdateOneAsync(filter, update);

			return updateResult?.ModifiedCount > 0;
		}
	}
}
