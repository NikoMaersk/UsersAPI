using MongoDB.Bson;
using UsersAPI.Model;

namespace UsersAPI.Repository.Interfaces
{
    public interface IAdminRepository
    {
        Task Add(Admin admin);
        Task Delete(ObjectId id);
        Task<Admin> Get(ObjectId id);
    }
}
