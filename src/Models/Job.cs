using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace mongoserialization.Models
{
    public class Job
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("metadata")]
        public BsonDocument Metadata { get; set; }

        [BsonElement("logs")]
        public List<LogMessage> Logs { get; set; }

        public Job()
        {
            Logs = new List<LogMessage>();
        }
    }
}