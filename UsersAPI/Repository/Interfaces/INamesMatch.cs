using UsersAPI.Model;

namespace UsersAPI.Repository.Interfaces
{
    public interface INamesMatch
    {
        Task<List<Match>> GetAll();
    }
}
