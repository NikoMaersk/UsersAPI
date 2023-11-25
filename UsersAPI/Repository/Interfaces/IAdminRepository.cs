using MongoDB.Bson;
using UsersAPI.Model;

namespace UsersAPI.Repository.Interfaces
{
	public interface IAdminRepository : IAuthorize
    {
        Task AddAsync(RegistrationRequest request);
		Task DeleteAsync(ObjectId id);
        Task<Admin> GetAsync(ObjectId id);
    }
}
