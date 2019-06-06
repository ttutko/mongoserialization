using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace mongoserialization.Models
{
    public class MyModelNewton
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        //[JsonProperty("_id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("jobs")]
        public List<JobNewton> Jobs { get; set; }

        [BsonElement("metadata")]
        [JsonProperty("metadata")]
        public object Metadata { get; set; }

        public MyModelNewton()
        {
            Jobs = new List<JobNewton>();
        }
    }
}
