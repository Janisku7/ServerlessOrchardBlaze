using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazeOrchard
{
    public class AuthToken
    {
        [JsonPropertyName("authenticationToken")]
        public string AuthenticationToken { get; set; }
        [JsonPropertyName("user")]
        public AuthenticationUser User { get; set; }
    }
    public class AuthenticationUser
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
    }
}
