using Newtonsoft.Json;

namespace AndersonDiego.Models
{
    public class ResponseError
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        [JsonIgnore]
        public bool ContainsError { get; set; }
    }
}