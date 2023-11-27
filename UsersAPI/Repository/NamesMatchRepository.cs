using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UsersAPI.Model;
using UsersAPI.Repository.Interfaces;

namespace UsersAPI.Repository
{
	public class NamesMatchRepository : INamesMatchRepository
	{
		private IMongoCollection<NameMatch> _matchCollection;

        public NamesMatchRepository(IOptions<DBSettings> mongoDB)
        {
            var client = new MongoClient(mongoDB.Value.ConnectionString);
            var mongoDatabase = client.GetDatabase(mongoDB.Value.DatabaseName);
            _matchCollection = mongoDatabase.GetCollection<NameMatch>(mongoDB.Value.MatchesCollectionName);
        }

		public async Task<List<NameMatch>> GetAllAsync()
		{
			return await _matchCollection.Find(item => true).ToListAsync();
		}

		public async Task<List<NameMatch>> GetAllByNameAsync(string name)
		{
			return await _matchCollection.Find(m => m.Name == name).ToListAsync();
		}

		public async Task AddAsync(NameMatch match)
		{
			await _matchCollection.InsertOneAsync(match);
		}
	}
}
