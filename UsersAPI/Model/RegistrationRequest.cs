using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Model
{
	public class RegistrationRequest
	{
		[Required]
		public string Name { get; set; } = string.Empty;
		[Required]
		public string Email { get; set; } = string.Empty;
		[Required]
		public string Password { get; set; } = string.Empty;

        public RegistrationRequest() { }

		public RegistrationRequest(string name, string email, string password)
		{
			this.Name = name;
			this.Email = email;
			this.Password = password;
		}
	}
}
