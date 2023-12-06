using MongoDB.Bson;
using UsersAPI.Model;

namespace UsersAPI.Repository.Interfaces
{
	public interface IAdminRepository : IAuthorize
    {
        Task AddAsync(string name, string email, string password);
		Task DeleteAsync(ObjectId id);
        Task<Admin> GetAsync(ObjectId id);
    }
}
