﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UsersAPI.Model
{
	public class NameMatch
	{
		[BsonId]
		public ObjectId Id { get; set; }
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		[BsonElement("date")]
		public DateTime Date { get; set; }
		[BsonElement("name")]
		public string Name { get; set; }

	}
}
