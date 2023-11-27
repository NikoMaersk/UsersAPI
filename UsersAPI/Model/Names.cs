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
		[BsonRepresentation(BsonType.String)]
		public string Name { get; set; }
		[Required]
		[BsonElement("gender")]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Gender Gender { get; set; }
		[Required]
		[BsonElement("international")]
		public bool IsInternational { get; set; }
		[BsonElement("popularity")]
		public int Popularity { get; set; }
		[BsonElement("occurrence")]
		public int Occurrence {  get; set; }
    }
}
