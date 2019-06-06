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
            //if(value == null)
            //{
            //    context.Writer.WriteNull();
            //    return;
            //}

            //if(value.GetType().IsAssignableFrom(typeof(JObject)))
            //{
            //    var jobj = (JObject)value;
            //    context.Writer.WriteStartDocument();
            //    foreach(var val in jobj)
            //    {
            //        switch(val.Value.Type)
            //        {
            //            case JTokenType.String:
            //                context.Writer.WriteName(val.Key);
            //                context.Writer.WriteString(val.Value.ToString());
            //                break;
            //            case JTokenType.Integer:
            //                context.Writer.WriteName(val.Key);
            //                context.Writer.WriteInt32((int)val.Value);
            //                break;
            //            case JTokenType.Float:
            //                context.Writer.WriteName(val.Key);
            //                context.Writer.WriteDouble((double)val.Value);
            //                break;
            //            case JTokenType.Array:

            //            default:    
            //                break;
            //        }
            //    }
            //    context.Writer.WriteEndDocument();
            //}
            //else
            //{
            //    context.Writer.WriteNull();
            //}
            SerializeJObject(context, args, value);
        }

        private void SerializeJObject(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value == null)
            {
                context.Writer.WriteNull();
                return;
            }

            if (value.GetType().IsAssignableFrom(typeof(JObject)))
            {
                var jobj = (JObject)value;
                context.Writer.WriteStartDocument();
                foreach (var val in jobj)
                {
                    context.Writer.WriteName(val.Key);
                    SerializeJObject(context, args, val.Value);
                    //switch (val.Value.Type)
                    //{
                    //    case JTokenType.String:
                    //        context.Writer.WriteName(val.Key);
                    //        context.Writer.WriteString(val.Value.ToString());
                    //        break;
                    //    case JTokenType.Integer:
                    //        context.Writer.WriteName(val.Key);
                    //        context.Writer.WriteInt32((int)val.Value);
                    //        break;
                    //    case JTokenType.Float:
                    //        context.Writer.WriteName(val.Key);
                    //        context.Writer.WriteDouble((double)val.Value);
                    //        break;
                    //    case JTokenType.Array:
                    //        context.Writer.WriteStartArray();
                    //        foreach(var elem in val.Value.Values())
                    //        {
                    //            SerializeJObject(context, args, elem);
                    //        }
                    //        context.Writer.WriteEndArray();
                    //        break;
                    //    default:
                    //        break;
                    //}
                }
                context.Writer.WriteEndDocument();
            }
            else if (value.GetType().IsAssignableFrom(typeof(JArray)))
            {
                var jarr = (JArray)value;
                context.Writer.WriteStartArray();

                foreach(var elem in jarr)
                {
                    SerializeJObject(context, args, elem);
                }

                context.Writer.WriteEndArray();
            }
            else if (value.GetType().IsAssignableFrom(typeof(JToken)))
            {
                return;
            }
            else if (value.GetType().IsAssignableFrom(typeof(JProperty)))
            {
                //context.Writer.WriteStartDocument();
                if (context.Writer.State == MongoDB.Bson.IO.BsonWriterState.Name)
                {
                    context.Writer.WriteName(((JProperty)value).Name);
                }
                SerializeJObject(context, args, ((JProperty)value).Value);

                
                //context.Writer.WriteEndDocument();
            }
            else if (value.GetType().IsAssignableFrom(typeof(JValue)))
            {
                var jval = (JValue)value;
                switch(jval.Type)
                {
                    case JTokenType.String:
                        context.Writer.WriteString(jval.Value.ToString());
                        break;
                    case JTokenType.Integer:
                        context.Writer.WriteInt64((long)jval.Value);
                        break;
                    case JTokenType.Float:
                        context.Writer.WriteDouble((double)jval.Value);
                        break;
                    case JTokenType.Date:
                        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        var dateTimeNoEpoch = ((DateTime)jval.Value).Subtract(new TimeSpan(epoch.Ticks));
                        var dateTimeNoEpochTicks = (long)dateTimeNoEpoch.Ticks / 10000;
                        context.Writer.WriteDateTime(dateTimeNoEpochTicks);
                        break;
                    case JTokenType.Null:
                        context.Writer.WriteNull();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                BsonSerializer.LookupSerializer(value.GetType()).Serialize(context, value);
            }
        }
    }
}
