using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Model
{
	public class RegistrationRequest
	{
		[Required]
		public string name { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }

        public RegistrationRequest() { }

		public RegistrationRequest(string name, string email, string password)
		{
			name = name;
			Email = email;
			Password = password;
		}
	}
}
