using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using UsersAPI.Model;
using UsersAPI.Repository.Interfaces;

namespace UsersAPI.Repository
{
    public class AdminRepository : IAdminRepository
	{
		private readonly IMongoCollection<Admin> _admins;

        public AdminRepository(IOptions<DBSettings> mongoDB)
        {
            var mongoClient = new MongoClient(mongoDB.Value.ConnectionString);
			var mongoDatabase = mongoClient.GetDatabase(mongoDB.Value.DatabaseName);
			_admins = mongoDatabase.GetCollection<Admin>(mongoDB.Value.AdminCollectionName);
        }

        public async Task Add(Admin admin)
		{
			await _admins.InsertOneAsync(admin);
		}

		public async Task Delete(ObjectId id)
		{
			await _admins.DeleteOneAsync(a => a.Id == id);
		}

		public async Task<Admin> Get(ObjectId id)
		{
			return await _admins.Find(a => a.Id == id).FirstOrDefaultAsync();
		}
	}
}
