using Microsoft.Extensions.Options;
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

			var indexKeysDefinitionName = Builders<Names>.IndexKeys.Ascending(n => n.Name);
			var indexModelName = new CreateIndexModel<Names>(indexKeysDefinitionName);
			_names.Indexes.CreateOne(indexModelName);
		}


        public async Task AddAsync(Names name)
		{
			string formattedName = char.ToUpper(name.Name[0]) + name.Name.Substring(1).ToLower();

			Names formattedNameObject = new Names
			{
				Name = formattedName,
				Gender = name.Gender,
				IsInternational = name.IsInternational,
				Popularity = name.Popularity,
				Occurrence = name.Occurrence
			};

			await _names.InsertOneAsync(formattedNameObject);
		}

		public async Task DeleteAsync(string name)
		{
			await _names.DeleteOneAsync(n => string.Equals(n.Name, name, StringComparison.OrdinalIgnoreCase));
		}

		async Task<List<Names>> INamesRepository.GetAllAsync()
		{
			return await _names.Find(n => true).ToListAsync();
		}

		public async Task<List<Names>> GetNamesSortedAsync(string sortField, string sortOrder)
		{
			var sortDefinition = GetSortDefinition(sortField, sortOrder);
			return await _names.Find(Builders<Names>.Filter.Empty).Sort(sortDefinition).ToListAsync();
		}

		public async Task<Names> GetByNameAsync(string name)
		{
			return await _names.Find(n => string.Equals(n.Name, name, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();
		}

		public async Task<List<Names>> GetByInternationalAsync(bool isInternational)
		{
			return await _names.Find(n => n.IsInternational == isInternational).ToListAsync();
		}

		public async Task<List<Names>> GetByGenderAsync(Gender gender)
		{
			return await _names.Find(n => n.Gender == gender).ToListAsync();
		}

		private SortDefinition<Names> GetSortDefinition(string sortField, string sortOrder)
		{
			var direction = sortOrder == "asc" ? 1 : -1;


			return sortField?.ToLower() switch
			{
				"Name" => direction == 1 ? Builders<Names>.Sort.Ascending(n => n.Name) : Builders<Names>.Sort.Descending(n => n.Name),
				"gender" => direction == 1 ? Builders<Names>.Sort.Ascending(n => n.Gender) : Builders<Names>.Sort.Descending(n => n.Gender),
				"international" => direction == 1 ? Builders<Names>.Sort.Ascending(n => n.IsInternational) : Builders<Names>.Sort.Descending(n => n.IsInternational),
				_ => Builders<Names>.Sort.Ascending(n => n.Name)
			};
		}

		public async Task<bool> IsNameValidAsync(string name)
		{
			return await _names
				.Find(n => string.Equals(n.Name, name, StringComparison.OrdinalIgnoreCase))
				.AnyAsync();
		}
	}
}
