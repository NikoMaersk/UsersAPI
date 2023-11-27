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
		[BsonElement("name")]
		public string UserName { get; set; } = string.Empty;

		[Required]
		[BsonElement("email")]
		public string Email { get; set; } = string.Empty;

		[BsonElement("chosenNames")]
		public List<string> Names { get; set; }

		[BsonElement("partner")]
		public string Partner { get; set; }

		[BsonElement("hashedPassword")]
		public string HashedPassword { get; set; } = string.Empty;
		[BsonElement("salt")]
		public string Salt { get; set; } = string.Empty;

		public User(RegistrationRequest registrationRequest)
		{
			UserName = registrationRequest.Name;
			Email = registrationRequest.Email;
		}

        public User()
        {
            
        }
    }
}
