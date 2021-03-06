﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace mongoserialization.Models
{
    public class JobNewton
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("metadata")]
        [JsonProperty("metadata", NamingStrategyType = typeof(DefaultNamingStrategy))]
        public object Metadata { get; set; }

        [BsonElement("logs")]
        public List<LogMessage> Logs { get; set; }

        public JobNewton()
        {
            Logs = new List<LogMessage>();
        }
    }
}