using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using mongoserialization.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace mongoserialization.test
{
    public class SerializationTest
    {
        MongoClient client;
        IMongoDatabase db = null;
        IMongoCollection<MyModel> collection = null;

        private readonly ITestOutputHelper output;
        public SerializationTest(ITestOutputHelper output)
        {
            this.output = output;
            client = new MongoClient("mongodb://localhost:27017");
            db = client.GetDatabase("Serialization");
            collection = db.GetCollection<MyModel>("Models");

        }

        [Fact]
        public void Bson_ImportJsonDataModel()
        {

            var exists = collection.Find(model => model.Name.Equals("Bson Model 1")).FirstOrDefault();
            if (exists == null)
            {
                string jsonData;
                using (StreamReader reader = new StreamReader(@"C:\Users\jsodomka\source\repos\mongoserialization\data\Serialization.Models.Bson.json"))
                {
                    jsonData = reader.ReadToEnd();
                }

                output.WriteLine(jsonData);

                var document = BsonSerializer.Deserialize<List<MyModel>>(jsonData);
                collection.InsertMany(document);
            }
        }

        [Fact]
        public void Bson_ReadDataModel()
        {
            var result = collection.Find(model => model.Name.Equals("Bson Model 1")).FirstOrDefault();
        }

        [Fact]
        public void NewtonSoft_ImportJsonDataModel()
        {
            var exists = collection.Find(model => model.Name.Equals("Newton Model 1")).FirstOrDefault();
            if (exists == null)
            {
                string jsonData;
                using (StreamReader reader = new StreamReader(@"C:\Users\jsodomka\source\repos\mongoserialization\data\Serialization.Models.Newton.json"))
                {
                    jsonData = reader.ReadToEnd();
                }

                output.WriteLine(jsonData);

                var document = BsonSerializer.Deserialize<List<MyModel>>(jsonData);
                collection.InsertMany(document);
            }
        }

        [Fact]
        public void NewtonSoft_ReadDataModel()
        {
            var result = collection.Find(model => model.Name.Equals("Newton Model 1")).FirstOrDefault();
        }
    }
}
