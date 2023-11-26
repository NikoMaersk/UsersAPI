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
		[BsonElement("Name")]
		public string UserName { get; set; } = string.Empty;

		[Required]
		[BsonElement("Email")]
		public string Email { get; set; } = string.Empty;

		[BsonElement("ChosenNames")]
		public List<string> Names { get; set; }

		[BsonElement("Partner")]
		public string Partner { get; set; }

		[BsonElement("HashedPassword")]
		public string HashedPassword { get; set; } = string.Empty;
		[BsonElement("Salt")]
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
