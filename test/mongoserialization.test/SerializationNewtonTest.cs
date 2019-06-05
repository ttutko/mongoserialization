using MongoDB.Bson;
using MongoDB.Driver;
using mongoserialization.Models;
using Newtonsoft.Json;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace mongoserialization.test
{
    public class SerializationNewtonTest
    {
        private IMongoClient client;
        private IMongoDatabase db = null;
        private IMongoCollection<MyModel> collectionModel = null;
        private IMongoCollection<MyModelNewton> collectionModelNewton = null;
        private IMongoCollection<BsonDocument> collectionBson = null;
        private readonly ITestOutputHelper output;

        public SerializationNewtonTest(ITestOutputHelper output)
        {
            this.output = output;
            client = new MongoClient("mongodb://localhost:27017");
            db = client.GetDatabase("Serialization");
            collectionModelNewton = db.GetCollection<MyModelNewton>("Models");
            collectionModel = db.GetCollection<MyModel>("Models");
            collectionBson = db.GetCollection<BsonDocument>("Models");
        }

        [Fact]
        public void NewtonSoft_ImportjsonModel()
        {
            var exists = collectionModelNewton.Find(model => model.Name.Equals("Newton Model 1")).FirstOrDefault();
            if (exists == null)
            {
                string json;
                var file = @"Data\Serialization.Models.Newton.json";
                using (StreamReader reader = new StreamReader(file))
                {
                    json = reader.ReadToEnd().Replace("\n", "");
                }

                // Show in Output Window
                output.WriteLine(json);

                var document = JsonConvert.DeserializeObject<MyModelNewton>(json);
                collectionModelNewton.InsertOne(document);
            }
        }

        [Fact]
        public void NewtonSoft_ReadDataModel()
        {
            
            var result = collectionModel.Find(model => model.Name.Equals("Newton Model 1")).FirstOrDefault();
            
            string json = JsonConvert.SerializeObject(result, Formatting.Indented);
            output.WriteLine(json);
            
            var jsonReplace = json.Replace("\r\n", "");
            output.WriteLine(jsonReplace);
        }
    }
}