using Newtonsoft.Json;

namespace vssAdminSPA.vssJSONMODELS
{
    public class vssJSON
    {
        public JsonSerializerSettings Settings { get; set; }
        public vssJSON(JsonSerializerSettings settings = null)
        {
            this.Settings = settings ?? new JsonSerializerSettings
                                            {
                                                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                                                DateParseHandling = DateParseHandling.None,
                                            };
        }
        public T ToObject<T>(string json) => JsonConvert.DeserializeObject<T>(json, this.Settings);

        public string ToJson<T>(T obj) => JsonConvert.SerializeObject(obj, this.Settings);        
    }
}
