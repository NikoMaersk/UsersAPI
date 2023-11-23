using UsersAPI.Model;
using MongoDB.Bson;

namespace UsersAPI.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task Add(RegistrationRequest request);
        Task Add(User user);
        Task<User> Get(ObjectId id);
        Task<List<User>> GetAll();
        Task Delete(ObjectId id);
        Task<ObjectId> GetIdFromEmail(string email);
        Task<bool> Authenticate(string email, string pass);
	}
}
