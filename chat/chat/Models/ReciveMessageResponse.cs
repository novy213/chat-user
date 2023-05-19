using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Models
{
    public class ReciveMessageResponse : APIResponse
    {
        [JsonProperty("messages")]
        public List<Message> Messages;
    }
}
