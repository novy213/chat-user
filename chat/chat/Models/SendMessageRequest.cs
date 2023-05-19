using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Models
{
    public class SendMessageRequest
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
