using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Models
{
    public class LoginResponse : APIResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("userId")]
        public int User_id { get; set; }
    }
}
