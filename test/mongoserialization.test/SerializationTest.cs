using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using mongoserialization.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace mongoserialization.test
{
    public class SerializationTest
    {
        private IMongoClient client;
        private IMongoDatabase db = null;
        private IMongoCollection<MyModel> collectionModel = null;
        private IMongoCollection<BsonDocument> collectionBson = null;
        private readonly ITestOutputHelper output;

        public SerializationTest(ITestOutputHelper output)
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
            var result = collectionModel.Find(model => model.Name.Equals("Bson Model 1")).FirstOrDefault();
        }

        [Fact]
        public void NewtonSoft_ImportjsonModel()
        {
            var exists = collectionModel.Find(model => model.Name.Equals("Newton Model 1")).FirstOrDefault();
            if (exists == null)
            {
                string json;
                var file = @"Data\Serialization.Models.Newton.json";
                using (StreamReader reader = new StreamReader(file))
                {
                    json = reader.ReadToEnd();
                }

                // Show in Output Window
                output.WriteLine(json);

                var document = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MyModel>>(json);
                collectionModel.InsertMany(document);
            }
        }

        [Fact]
        public void NewtonSoft_ReadDataModel()
        {
            MyModel myModel = collectionModel.Find(model => model.Name.Equals("Bson Model 1")).FirstOrDefault();
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(myModel, Formatting.Indented);
            output.WriteLine(json);            
        }
    }
}
