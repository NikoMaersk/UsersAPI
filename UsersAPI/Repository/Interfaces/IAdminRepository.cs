using MongoDB.Bson;
using UsersAPI.Model;

namespace UsersAPI.Repository.Interfaces
{
    public interface IAdminRepository : IAuthorize
    {
        Task Add(RegistrationRequest request);
		Task Delete(ObjectId id);
        Task<Admin> Get(ObjectId id);
    }
}
