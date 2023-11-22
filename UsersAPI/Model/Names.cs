using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace UsersAPI.Model
{
	public enum Gender
	{
		Boy,
		Girl,
		Uni
	}

	public class Names
	{
		[BsonId]
		public ObjectId Id { get; set; }
		[BsonElement("Name")]
		public string Name { get; set; }
		[BsonElement("Gender")]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Gender Gender { get; set; }
		[BsonElement("International")]
		public bool IsInternational { get; set; }
		[BsonElement("Popularity")]
		public int Popularity { get; set; }
		[BsonElement("Occurrence")]
		public int Occurrence {  get; set; }

		public Names(ObjectId id, string name, Gender gender, bool isInternational, int popularity, int occurrence)
		{
			Id = id;
			Name = name;
			Gender = gender;
			IsInternational = isInternational;
			Popularity = popularity;
			Occurrence = occurrence;
		}
	}
}
