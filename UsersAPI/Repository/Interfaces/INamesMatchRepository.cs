using UsersAPI.Model;

namespace UsersAPI.Repository.Interfaces
{
	public interface INamesMatchRepository
    {
        Task<List<Match>> GetAllAsync();
        Task<List<Match>> GetAllByNameAsync(string name);
        Task AddAsync(Match match);
    }
}
