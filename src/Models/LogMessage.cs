using MongoDB.Bson.Serialization.Attributes;
using System;

namespace mongoserialization.Models
{
    public class LogMessage
    {
        [BsonElement("message")]
        public string Message { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}