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
		public string Name { get; set; } = string.Empty;
		[Required]
		[BsonElement("HashedPassword")]
		public string HashedPassword { get; set; } = string.Empty;
		[BsonElement("Salt")]
		public string Salt { get; set; } = string.Empty;
	}
}
