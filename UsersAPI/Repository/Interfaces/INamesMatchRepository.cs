using UsersAPI.Model;

namespace UsersAPI.Repository.Interfaces
{
	public interface INamesMatchRepository
    {
        Task<List<NameMatch>> GetAllAsync();
        Task<List<NameMatch>> GetAllByNameAsync(string name);
        Task AddAsync(NameMatch match);
    }
}
