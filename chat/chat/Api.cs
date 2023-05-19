using chat.Models;
using chat.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unirest_net.http;
using unirest_net.request;

namespace chat
{
    public class Api
    {
        private const string HEADER_AUTHORIZATION = "Authorization";
        private const string API_ENDPOINT_LOGOUT = "/chat";
        private const string API_ENDPOINT_LOGIN = "/chat";
        private const string API_ENDPOINT_REGISTER = "/chat/register";
        private const string API_ENDPOINT_GET_USERS = "/chat/user";
        private const string API_ENDPOINT_RECIVE_MESSAGE = "/chat/message";
        private const string API_ENDPOINT_SEND_MESSAGE = "/chat/message";
        private const string API_ENDPOINT_GET_ALL_USERS = "/chat";

        private const int HTTP_STATUS_OK = 200;
        private const int HTTP_STATUS_UNAUTHORIZED = 401;

        private static String Message;
        public static async Task<GetUsersResponse> GetAllUsersAsync()
        {
            GetUsersResponse response;
            HttpRequest req = Unirest.get(Settings.Default.url + API_ENDPOINT_GET_ALL_USERS);
            try
            {
                HttpResponse<String> rawResponse = await MakeRequestAsync(req);
                if (rawResponse == null)
                {
                    response = new GetUsersResponse
                    {
                        Error = true,
                        Message = Message
                    };
                }
                else
                {
                    response = JsonConvert.DeserializeObject<GetUsersResponse>(rawResponse.Body);
                }

            }
            catch (Exception)
            {
                response = new GetUsersResponse
                {
                    Error = true,
                    Message = "Unable to deserialize response from remote server"
                };
            }
            return response;
        }
        public static async Task<APIResponse> SendMessageAsync(int User_to, string Message)
        {
            APIResponse response;
            SendMessageRequest body = new SendMessageRequest
            {
                Message = Message,
            };
            HttpRequest req = Unirest.post(Settings.Default.url + API_ENDPOINT_SEND_MESSAGE + "/" + User_to).body<SendMessageRequest>(body);
            try
            {
                HttpResponse<String> rawResponse = await MakeRequestAsync(req);
                if (rawResponse == null)
                {
                    response = new APIResponse
                    {
                        Error = true,
                        Message = Message
                    };
                }
                else
                {
                    response = JsonConvert.DeserializeObject<APIResponse>(rawResponse.Body);
                }

            }
            catch (Exception)
            {
                response = new APIResponse
                {
                    Error = true,
                    Message = "Unable to deserialize response from remote server"
                };
            }
            return response;
        }
        public static async Task<ReciveMessageResponse> ReciveMessageAsync(int User_from)
        {
            ReciveMessageResponse response;
            HttpRequest req = Unirest.get(Settings.Default.url + API_ENDPOINT_RECIVE_MESSAGE + "/" + User_from);
            try
            {
                HttpResponse<String> rawResponse = await MakeRequestAsync(req);
                if (rawResponse == null)
                {
                    response = new ReciveMessageResponse
                    {
                        Error = true,
                        Message = Message
                    };
                }
                else
                {
                    response = JsonConvert.DeserializeObject<ReciveMessageResponse>(rawResponse.Body);
                }

            }
            catch (Exception)
            {
                response = new ReciveMessageResponse
                {
                    Error = true,
                    Message = "Unable to deserialize response from remote server"
                };
            }
            return response;
        }
        public static async Task<GetUsersResponse> GetUsersAsync()
        {
            GetUsersResponse response;
            HttpRequest req = Unirest.get(Settings.Default.url + API_ENDPOINT_GET_USERS);
            try
            {
                HttpResponse<String> rawResponse = await MakeRequestAsync(req);
                if (rawResponse == null)
                {
                    response = new GetUsersResponse
                    {
                        Error = true,
                        Message = Message
                    };
                }
                else
                {
                    response = JsonConvert.DeserializeObject<GetUsersResponse>(rawResponse.Body);
                }

            }
            catch (Exception)
            {
                response = new GetUsersResponse
                {
                    Error = true,
                    Message = "Unable to deserialize response from remote server"
                };
            }
            return response;
        }

        public static async Task<APIResponse> RegisterAsync(string Login, string Password, string Name, string Last_name)
        {
            APIResponse response;
            RegisterRequest request = new RegisterRequest
            {
                Login = Login,
                Password = Password,
                Name = Name,
                Last_name = Last_name
            };
            HttpRequest req = Unirest.post(Settings.Default.url + API_ENDPOINT_REGISTER).body<RegisterRequest>(request);
            try
            {
                HttpResponse<String> rawResponse = await MakeRequestAsync(req);
                if (rawResponse == null)
                {
                    response = new APIResponse
                    {
                        Error = true,
                        Message = Message
                    };
                }
                else
                {
                    response = JsonConvert.DeserializeObject<APIResponse>(rawResponse.Body);
                }

            }
            catch (Exception)
            {
                response = new APIResponse
                {
                    Error = true,
                    Message = "Unable to deserialize response from remote server"
                };
            }
            return response;
        }
        public static async Task<LoginResponse> LoginAsync(string Login, string Password)
        {
            LoginResponse response;
            LoginRequest loginInfo = new LoginRequest
            {
                Login = Login,
                Password = Password
            };
            HttpRequest req = Unirest.post(Settings.Default.url+ API_ENDPOINT_LOGIN).body<LoginRequest>(loginInfo);
            try
            {
                HttpResponse<String> rawResponse = await MakeRequestAsync(req);
                if (rawResponse == null)
                {
                    response = new LoginResponse
                    {
                        Error = true,
                        Message = Message
                    };
                }
                else
                {
                    response = JsonConvert.DeserializeObject<LoginResponse>(rawResponse.Body);
                    if (rawResponse.Code == HTTP_STATUS_OK)
                    {
                        Settings.Default.accessToken = response.Token;
                        Settings.Default.user_id = response.User_id;
                        Settings.Default.Save();
                    }
                }

            }
            catch (Exception)
            {
                response = new LoginResponse
                {
                    Error = true,
                    Message = "Unable to deserialize response from remote server"
                };
            }
            return response;
        }
        public static async Task<APIResponse> LogoutAsync()
        {
            APIResponse response;
            HttpRequest req = Unirest.delete(Settings.Default.url + API_ENDPOINT_LOGOUT);
            try
            {
                HttpResponse<String> rawResponse = await MakeRequestAsync(req);
                if (rawResponse == null)
                {
                    response = new LoginResponse
                    {
                        Error = true,
                        Message = Message
                    };
                }
                else
                {
                    response = JsonConvert.DeserializeObject<APIResponse>(rawResponse.Body);
                    if (rawResponse.Code == HTTP_STATUS_OK)
                    {
                        Settings.Default.accessToken = "";
                        Settings.Default.user_id = 0;
                        Settings.Default.Save();
                    }
                }

            }
            catch (Exception)
            {
                response = new LoginResponse
                {
                    Error = true,
                    Message = "Unable to deserialize response from remote server"
                };
            }
            return response;
        }
        public static string GetAuthHeaders()
        {
            return HEADER_AUTHORIZATION + ": " + "Bearer " + Settings.Default.accessToken;
        }
        private static async Task<HttpResponse<string>> MakeRequestAsync(HttpRequest req)
        {
            Task<HttpResponse<String>> resultTask;
            HttpResponse<String> result;
            if (Settings.Default.accessToken != "")
            {
                req.header(HEADER_AUTHORIZATION, "Bearer " + Settings.Default.accessToken);
            }
            Message = "";
            resultTask = req.asStringAsync();
            try
            {
                result = await resultTask;
                if (result.Code == HTTP_STATUS_UNAUTHORIZED)
                {
                    Settings.Default.accessToken = "";
                    result.Body = "{\"error\":false, \"message\":\"\"}";
                    Settings.Default.Save();
                }
            }
            catch (AggregateException ae)
            {
                Message = ae.InnerException.Message;
                result = null;
            }
            catch (Exception e)
            {
                Message = e.Message;
                result = null;
            }
            return result;
        }
    }
}
