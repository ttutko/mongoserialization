using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mongoserialization.Serializers
{
    [BsonSerializer(typeof(MyCustomSerializer))]
    public class MyCustomSerializer : IBsonSerializer
    {
        public Type ValueType => typeof(object);

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return BsonSerializer.LookupSerializer<object>().Deserialize(context);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if(value == null)
            {
                context.Writer.WriteNull();
                return;
            }

            if(value.GetType().IsAssignableFrom(typeof(JObject)))
            {
                var jobj = (JObject)value;
                context.Writer.WriteStartDocument();
                foreach(var val in jobj)
                {
                    context.Writer.WriteName(val.Key);
                    switch(val.Value.Type)
                    {
                        case JTokenType.String:                            
                            context.Writer.WriteString(val.Value.ToString());
                            break;
                        case JTokenType.Integer:
                            context.Writer.WriteInt32((int)val.Value);
                            break;
                        case JTokenType.Float:
                            context.Writer.WriteDouble((double)val.Value);
                            break;
                        default:    
                            break;
                    }
                }
                context.Writer.WriteEndDocument();
            }
        }
    }
}
