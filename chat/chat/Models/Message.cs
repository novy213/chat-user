using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Models
{
    public class Message
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("message")]
        public string MessageText { get; set; }
        [JsonProperty("from")]
        public string From { get; set; }
    }
}
