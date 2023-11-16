using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UsersAPI.Model
{
	public class User
	{
		[BsonId]
		public Guid Id { get; set; }

		[BsonElement("UserId")]
		[BsonRepresentation(BsonType.Int32)]
		public int UserId { get; set; }

		[BsonElement("UserName")]
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }

		public User(Guid id, int userId, string userName, string password, string email)
		{
			Id = id;
			UserId = userId;
			UserName = userName;
			Password = password;
			Email = email;
		}
	}
}
