using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mongoserialization.Models
{
    public class MyModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        //[JsonProperty("_id")]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("jobs")]
        public List<Job> Jobs { get; set; }

        [BsonElement("metadata")]
        public object Metadata { get; set; }

        public MyModel()
        {
            Jobs = new List<Job>();
        }
    }
}
