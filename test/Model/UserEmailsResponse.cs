using Newtonsoft.Json;

namespace AT.Github.Automation.Tests
{
    public class UserEmailsResponse
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("primary")]
        public bool Primary { get; set; }
        [JsonProperty("verified")]
        public bool Verified { get; set; }
    }
}
