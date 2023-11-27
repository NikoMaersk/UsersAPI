using UsersAPI.Model;

namespace UsersAPI.Repository.Interfaces
{
	public interface INamesRepository
    {
        Task AddAsync(Names name);
        Task<Names> GetByNameAsync(string name);
        Task<List<Names>> GetByInternationalAsync(bool isInternational);
        Task<List<Names>> GetByGenderAsync(Gender gender);
        Task<List<Names>> GetAllAsync();
        Task DeleteAsync(string name);
        Task<List<Names>> GetNamesSortedAsync(string sortField, string sortOrder);
		Task<bool> IsNameValidAsync(string name);

	}
}
