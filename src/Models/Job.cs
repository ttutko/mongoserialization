using MongoDB.Bson;
using System.Collections.Generic;

namespace mongoserialization.Models
{
    public class Job
    {
        public string Name { get; set; }
        public BsonDocument Metadata { get; set; }
        public List<LogMessage> Logs { get; set; }

        public Job()
        {
            Logs = new List<LogMessage>();
        }
    }
}