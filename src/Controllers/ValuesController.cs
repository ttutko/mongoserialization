using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using mongoserialization.Models;

namespace mongoserialization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMongoClient mongoClient;

        public ValuesController(IMongoClient mongoClient)
        {
            this.mongoClient = mongoClient;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<MyModel>> Get()
        {
            IMongoCollection<MyModel> modelCollection = mongoClient.GetDatabase("Serialization").GetCollection<MyModel>("Models");
            var filter = Builders<MyModel>.Filter.Empty;
            return Ok(modelCollection.Find(filter).ToList());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<MyModel> Get(string id)
        {
            IMongoCollection<MyModel> modelCollection = mongoClient.GetDatabase("Serialization").GetCollection<MyModel>("Models");
            var filter = Builders<MyModel>.Filter.Eq(m => m.Id, id);
            return Ok(modelCollection.Find(filter).SingleOrDefault());
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
