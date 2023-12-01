using MongoDB.Bson;
using UsersAPI.Model;

namespace UsersAPI.Repository.Interfaces
{
	public interface INamesMatchRepository
    {
        Task<List<NameMatch>> GetAllAsync();
        Task<List<NameMatch>> GetAllByNameAsync(string name);
        Task<NameMatch> GetAsync(ObjectId id);
        Task AddAsync(NameMatch match);
		Task DeleteAsync(ObjectId id);
	}
}
