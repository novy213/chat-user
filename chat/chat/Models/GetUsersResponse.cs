using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Models
{
    public class GetUsersResponse : APIResponse
    {
        [JsonProperty("users")]
        public List<User> Users;
    }
}
