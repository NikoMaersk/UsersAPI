using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Model
{
	public class Admin
	{
		[BsonId]
		public ObjectId Id { get; set; }
		[Required]
		[BsonElement("Name")]
		public string Name { get; set; }
		[Required]
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
