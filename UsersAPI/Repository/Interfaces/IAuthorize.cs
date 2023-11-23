namespace UsersAPI.Repository.Interfaces
{
	public interface IAuthorize
	{
		Task<bool> Authenticate(string email, string pass);
	}
}
