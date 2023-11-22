using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UsersAPI.Model;
using UsersAPI.Repository.Interfaces;

namespace UsersAPI.Repository
{
	public class NamesMatchRepository : INamesMatch
	{
		private IMongoCollection<Match> _matchCollection;

        public NamesMatchRepository(IOptions<DBSettings> mongoDB)
        {
            var client = new MongoClient(mongoDB.Value.ConnectionString);
            var mongoDatabase = client.GetDatabase(mongoDB.Value.DatabaseName);
            _matchCollection = mongoDatabase.GetCollection<Match>(mongoDB.Value.MatchesCollectionName);
        }

		public async Task<List<Match>> GetAll()
		{
			return await _matchCollection.Find(item => true).ToListAsync();
		}
	}
}
