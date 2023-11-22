using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UsersAPI.Model
{
	public class User
	{
		[BsonId]
		public ObjectId Id { get; set; }
		[BsonElement("UserName")]
		public string UserName { get; set; }
		public string Password { get; set; }
		[BsonElement("Email")]
		public string Email { get; set; }
		[BsonElement("Chosen_Names")]
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
