using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Xml.Linq;
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
			string formattedName = char.ToUpper(match.Name[0]) + match.Name.Substring(1).ToLower();

			var formattedMatch = new NameMatch
			{
				Date = match.Date,
				Name = formattedName,
			};

			await _matchCollection.InsertOneAsync(formattedMatch);
		}

		public async Task<NameMatch> GetAsync(ObjectId id)
		{
			return await _matchCollection.Find(m => m.Id == id).FirstOrDefaultAsync();
		}

		public async Task DeleteAsync(ObjectId id)
		{
			await _matchCollection.DeleteOneAsync(m =>  m.Id == id);
		}
	}
}
