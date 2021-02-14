using Newtonsoft.Json;

namespace AndersonDiego.Models
{
    public class Response
    {
        [JsonProperty("message")]
        public string Message{get;set;}
    }
}