using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UsersAPI.Model
{
	public class Admin
	{
		[BsonId]
		public ObjectId Id { get; set; }
		[BsonElement("Name")]
		public string Name { get; set; }
		[BsonElement("Password")]
		public string Password { get; set; }

		public Admin(ObjectId id, string name, string password)
		{
			Id = id;
			Name = name;
			Password = password;
		}
	}
}
