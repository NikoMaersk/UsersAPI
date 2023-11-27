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
		[BsonElement("name")]
		public string Name { get; set; }
		[Required]
		[BsonElement("email")]
		public string Email { get; set; }
		[BsonElement("hashedPassword")]
		public string HashedPassword { get; set; } = string.Empty;
		[BsonElement("salt")]
		public string Salt { get; set; } = string.Empty;

		public Admin(RegistrationRequest registrationRequest)
		{
			Name = registrationRequest.Name;
			Email = registrationRequest.Email;
		}

        public Admin() {}
    }
}
