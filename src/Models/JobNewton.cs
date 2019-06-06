using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace mongoserialization.Models
{
    public class JobNewton
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("metadata")]
        public Object Metadata { get; set; }

        [BsonElement("logs")]
        public List<LogMessage> Logs { get; set; }

        public JobNewton()
        {
            Logs = new List<LogMessage>();
        }
    }
}