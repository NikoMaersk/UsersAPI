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
		public Guid Id { get; set; }
		[BsonElement("Name")]
		public string Name { get; set; }
		[BsonElement("Gender")]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Gender Gender { get; set; }

		public Names(Guid id, string name, Gender gender)
		{
			Id = id;
			Name = name;
			Gender = gender;
		}
	}
}
