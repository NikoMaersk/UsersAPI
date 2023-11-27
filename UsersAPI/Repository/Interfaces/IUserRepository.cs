using MongoDB.Bson;
using UsersAPI.Model;

namespace UsersAPI.Repository.Interfaces
{
	public interface IUserRepository : IAuthorize
    {
        Task AddAsync(RegistrationRequest request);
        Task<User> GetAsync(ObjectId id);
        Task<List<User>> GetAllAsync();
        Task DeleteAsync(ObjectId id);
        Task<User> GetByEmailAsync(string email);
        Task AddNameToUserAsync(string name, string email);
        Task PatchPartnerLink(string name);
	}
}
