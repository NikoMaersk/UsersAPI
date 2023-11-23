using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using UsersAPI.Model;
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

		public async Task<List<Names>> GetNamesSorted(string sortField, string sortOrder)
		{
			var sortDefinition = GetSortDefinition(sortField, sortOrder);
			return await _names.Find(Builders<Names>.Filter.Empty).Sort(sortDefinition).ToListAsync();
		}

		public async Task<Names> GetByName(string names)
		{
			return await _names.Find(n => string.Equals(n.Name, names, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();
		}

		public async Task<List<Names>> GetByInternational(bool isInternational)
		{
			return await _names.Find(n => n.IsInternational == isInternational).ToListAsync();
		}

		public async Task<List<Names>> GetByGender(Gender gender)
		{
			return await _names.Find(n => n.Gender == gender).ToListAsync();
		}

		private SortDefinition<Names> GetSortDefinition(string sortField, string sortOrder)
		{
			var direction = sortOrder == "asc" ? 1 : -1;


			return sortField?.ToLower() switch
			{
				"name" => direction == 1 ? Builders<Names>.Sort.Ascending(n => n.Name) : Builders<Names>.Sort.Descending(n => n.Name),
				"gender" => direction == 1 ? Builders<Names>.Sort.Ascending(n => n.Gender) : Builders<Names>.Sort.Descending(n => n.Gender),
				"international" => direction == 1 ? Builders<Names>.Sort.Ascending(n => n.IsInternational) : Builders<Names>.Sort.Descending(n => n.IsInternational),
				_ => Builders<Names>.Sort.Ascending(n => n.Name)
			};
		}
	}
}
