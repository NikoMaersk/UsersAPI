using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UsersAPI.Model
{
	public class Match
	{
		[BsonId]
		public ObjectId Id { get; set; }
		[BsonElement("Date")]
		public DateTime Date { get; set; }
		[BsonElement("Name")]
		public string Name { get; set; }

	}
}
