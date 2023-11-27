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
        Task PatchPartnerLinkAsync(string email, string linkEmail);
		Task<bool> ClearNamesListAsync(string email);
        Task<bool> ClearNameFromListAsync(string email, List<string> namesToRemove);
		Task<bool> CheckIfUserExistsAsync(string email);
        Task<bool> isNameAlreadyStoredAsync(string email, string name);
	}
}
