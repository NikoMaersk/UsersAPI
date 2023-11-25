namespace UsersAPI.Repository.Interfaces
{
	public interface IAuthorize
	{
		Task<bool> AuthenticateAsync(string email, string pass);
	}
}
