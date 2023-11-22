using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Model
{
	public class User
	{
		[BsonId]
		public ObjectId Id { get; set; }
		[Required]
		[BsonElement("Username")]
		public string UserName { get; set; } = string.Empty;
		// To be changed. Should not be plain text
		public string Password { get; set; }
		[Required]
		[BsonElement("Email")]
		public string Email { get; set; } = string.Empty;
		[BsonElement("ChosenNames")]
		public List<Names> names { get; set; }

		public User(ObjectId id, string userName, string password, string email, List<Names> names)
		{
			Id = id;
			UserName = userName;
			Password = password;
			Email = email;
			this.names = names;
		}
	}
}
