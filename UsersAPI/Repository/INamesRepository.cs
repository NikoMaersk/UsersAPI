using UsersAPI.Model;

namespace UsersAPI.Repository
{
	public interface INamesRepository
	{
		Task Add(Names name);
		Task<Names> Get(Guid id);
		Task<List<Names>> GetAll();
		Task Delete(Guid id);
		Task<List<Names>> GetNamesSortedByName();
		Task<List<Names>> GetNamesSortedByGender();
	}
}
