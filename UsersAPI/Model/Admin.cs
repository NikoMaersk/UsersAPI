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
		[BsonElement("Email")]
		public string Email { get; set; }
		[BsonElement("HashedPassword")]
		public string HashedPassword { get; set; } = string.Empty;
		[BsonElement("Salt")]
		public string Salt { get; set; } = string.Empty;

		public Admin(RegistrationRequest registrationRequest)
		{
			Name = registrationRequest.name;
			Email = registrationRequest.Email;
		}

        public Admin() {}
    }
}
