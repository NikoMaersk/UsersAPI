using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Model
{
	public class LoginRequest
	{
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
