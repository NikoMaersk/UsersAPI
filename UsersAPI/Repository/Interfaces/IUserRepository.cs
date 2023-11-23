using UsersAPI.Model;
using MongoDB.Bson;

namespace UsersAPI.Repository.Interfaces
{
    public interface IUserRepository : IAuthorize
    {
        Task Add(RegistrationRequest request);
        Task<User> Get(ObjectId id);
        Task<List<User>> GetAll();
        Task Delete(ObjectId id);
        Task<ObjectId> GetIdFromEmail(string email);
	}
}
