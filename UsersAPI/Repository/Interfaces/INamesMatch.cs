using UsersAPI.Model;

namespace UsersAPI.Repository.Interfaces
{
    public interface INamesMatch
    {
        Task<List<Match>> GetAll();
        Task<List<Match>> GetAllByName(string name);
    }
}
