using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Model
{
	public class RemoveNamesRequest
	{
		[Required]
		public List<string>? Names { get; set; }
	}
}
