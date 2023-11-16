using UsersAPI.Model;

namespace UsersAPI.Repository
{
	public interface IUserRepository
	{
		Task Add(User user);
		Task<User> Get(Guid id);
		Task<List<User>> GetAll();
		Task Delete(Guid id);
	}
}
