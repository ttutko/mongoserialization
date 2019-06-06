using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using mongoserialization.Models;
using mongoserialization.Serializers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            BsonClassMap.RegisterClassMap<MyModelNewton>(cm =>
            {
                cm.AutoMap();
                cm.MapProperty(c => c.Metadata).SetSerializer(new MyCustomSerializer());
            });

            BsonClassMap.RegisterClassMap<JobNewton>(cm =>
            {
                cm.AutoMap();
                cm.MapProperty(c => c.Metadata).SetSerializer(new MyCustomSerializer());
            });

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
            
            JObject actual = JObject.Parse(JsonConvert.SerializeObject(result, Formatting.Indented));
            var file = @"Data\Serialization.Models.Newton.json";
            JObject expected;
            using (StreamReader reader = new StreamReader(file))
            {
                expected = JObject.Parse(reader.ReadToEnd());
            }

            Assert.True(JObject.DeepEquals(actual, expected));
            //output.WriteLine(json);

            //var jsonReplace = json.Replace("\r\n", "");
            //output.WriteLine(jsonReplace);
        }
    }
}
