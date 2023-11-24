using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Model
{
	public class RegistrationRequest
	{
		[Required]
		public string name { get; set; } = string.Empty;
		[Required]
		public string Email { get; set; } = string.Empty;
		[Required]
		public string Password { get; set; } = string.Empty;

        public RegistrationRequest() { }

		public RegistrationRequest(string name, string email, string password)
		{
			name = name;
			Email = email;
			Password = password;
		}
	}
}
