using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UsersAPI.Model;
using MongoDB.Bson;
using UsersAPI.Repository.Interfaces;

namespace UsersAPI.Repository
{
    public class NamesRepository : INamesRepository
	{
		private readonly IMongoCollection<Names> _names;

        public NamesRepository(IOptions<DBSettings> mongoDB)
        {
			var mongoClient = new MongoClient(mongoDB.Value.ConnectionString);
			var mongoDatabase = mongoClient.GetDatabase(mongoDB.Value.DatabaseName);
			_names = mongoDatabase.GetCollection<Names>(mongoDB.Value.NamesCollectionName);
		}


        public async Task Add(Names name)
		{
			await _names.InsertOneAsync(name);
		}

		public async Task Delete(ObjectId id)
		{
			await _names.DeleteOneAsync(n => n.Id == id);
		}

		public async Task<Names> Get(ObjectId id)
		{
			return await _names.Find(n => n.Id == id).FirstOrDefaultAsync();
		}

		async Task<List<Names>> INamesRepository.GetAll()
		{
			return await _names.Find(n => true).ToListAsync();
		}

		public async Task<List<Names>> GetNamesSortedByName()
		{
			var sort = Builders<Names>.Sort.Ascending(n => n.Name);
			return await _names.Find(Builders<Names>.Filter.Empty).Sort(sort).ToListAsync();
		}

		public async Task<List<Names>> GetNamesSortedByGender()
		{
			var sort = Builders<Names>.Sort.Ascending(n => n.Gender);
			return await _names.Find(Builders<Names>.Filter.Empty).Sort(sort).ToListAsync();
		}
	}
}
