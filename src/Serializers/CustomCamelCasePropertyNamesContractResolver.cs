using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mongoserialization.Serializers
{
    public class CustomCamelCasePropertyNamesContractResolver : CamelCasePropertyNamesContractResolver
    {
        public CustomCamelCasePropertyNamesContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy { OverrideSpecifiedNames = false };
        }
    }
}
