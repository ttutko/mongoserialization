using MongoDB.Bson.Serialization.Attributes;
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
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Job> Jobs { get; set; }
        public object Metadata { get; set; }

        public MyModel()
        {
            Jobs = new List<Job>();
        }
    }
}
