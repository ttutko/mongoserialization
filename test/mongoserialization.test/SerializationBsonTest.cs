using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using mongoserialization.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace mongoserialization.test
{
    public class SerializationBsonTest
    {
        private IMongoClient client;
        private IMongoDatabase db = null;
        private IMongoCollection<MyModel> collectionModel = null;
        private IMongoCollection<BsonDocument> collectionBson = null;
        private readonly ITestOutputHelper output;

        public SerializationBsonTest(ITestOutputHelper output)
        {
            this.output = output;
            client = new MongoClient("mongodb://localhost:27017");
            db = client.GetDatabase("Serialization");
            collectionModel = db.GetCollection<MyModel>("Models");
            collectionBson = db.GetCollection<BsonDocument>("Models");
        }

        [Fact]
        public void Bson_ImportjsonModel()
        {
            var exists = collectionModel.Find(model => model.Name.Equals("Bson Model 1")).FirstOrDefault();
            if (exists == null)
            {
                string json;
                var file = @"Data\Serialization.Models.Bson.json";
                using (StreamReader reader = new StreamReader(file))
                {
                    json = reader.ReadToEnd();
                }

                // Show in Output Window
                output.WriteLine(json);

                var document = BsonSerializer.Deserialize<List<MyModel>>(json);
                collectionModel.InsertMany(document);
            }
        }

        [Fact]
        public void Bson_ReadDataModel()
        {
            //var result = collectionModel.Find(model => model.Name.Equals("Bson Model 1")).FirstOrDefault();

            var result = collectionModel.Find(model => model.Name.Equals("Bson Model 1")).FirstOrDefault();
            string json = JsonConvert.SerializeObject(result, Formatting.Indented);
            
            var jsonReplace = json.Replace("\r\n", "");
            output.WriteLine(jsonReplace);
        }

    }
}
