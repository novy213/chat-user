﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Models
{
    public class LoginRequest
    {
        [JsonProperty("login")]
        public string Login;

        [JsonProperty("password")]
        public string Password;
    }
}