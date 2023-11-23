using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UsersAPI.Model
{
	public enum Gender
	{
		boy,
		girl,
		uni
	}

	public class Names
	{
		[BsonId]
		public ObjectId Id { get; set; }
		[Required]
		[BsonElement("Name")]
		public string Name { get; set; }
		[Required]
		[BsonElement("Gender")]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Gender Gender { get; set; }
		[Required]
		[BsonElement("International")]
		public bool IsInternational { get; set; }
		[BsonElement("Popularity")]
		public int Popularity { get; set; }
		[BsonElement("Occurrence")]
		public int Occurrence {  get; set; }
    }
}
