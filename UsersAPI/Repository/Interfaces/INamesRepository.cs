using UsersAPI.Model;
using MongoDB.Bson;

namespace UsersAPI.Repository.Interfaces
{
    public interface INamesRepository
    {
        Task Add(Names name);
        Task<Names> Get(ObjectId id);
        Task<List<Names>> GetAll();
        Task Delete(ObjectId id);
        Task<List<Names>> GetNamesSortedByName();
        Task<List<Names>> GetNamesSortedByGender();
    }
}
