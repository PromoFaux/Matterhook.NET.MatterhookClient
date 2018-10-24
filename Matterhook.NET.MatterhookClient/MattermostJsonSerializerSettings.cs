using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Matterhook.NET.MatterhookClient
{
    public class MattermostJsonSerializerSettings : JsonSerializerSettings
    {
        public MattermostJsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore;
            ContractResolver=new CamelCasePropertyNamesContractResolver();
        }
    }
}