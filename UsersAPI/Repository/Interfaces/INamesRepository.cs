using UsersAPI.Model;
using MongoDB.Bson;

namespace UsersAPI.Repository.Interfaces
{
    public interface INamesRepository
    {
        Task Add(Names name);
        Task<Names> Get(ObjectId id);
        Task<Names> GetByName(string names);
        Task<List<Names>> GetByInternational(bool isInternational);
        Task<List<Names>> GetByGender(Gender gender);
        Task<List<Names>> GetAll();
        Task Delete(ObjectId id);
        Task<List<Names>> GetNamesSorted(string sortField, string sortOrder);
	}
}
