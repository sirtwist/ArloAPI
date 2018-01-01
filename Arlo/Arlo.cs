using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Arlo
{
    public class Arlo
    {
        public class ArloCreateResults
        {
            public Arlo Arlo { get; set; }
            public string Error { get; set; }
        }

        private LoginData authInfo = null;
        private string authUsername = null;
        private string authPassword = null;
        private Dictionary<string, string> headers = new Dictionary<string, string>();
        private Dictionary<string, string> parameters = new Dictionary<string, string>();
 
        private Arlo(string username, string password)
        {
            authUsername = username;
            authPassword = password;
        }

        public static async Task<ArloCreateResults> Create(string username, string password)
        {
            ArloCreateResults results = new ArloCreateResults();
            var arlo = new Arlo(username, password);
            string error = await arlo.Authenticate();
            if (arlo.authInfo != null)
            {
                results.Arlo = arlo;
                results.Error = null;
            } else
            {
                results.Arlo = null;
                results.Error = error;
            }
            return results;
        }

        private async Task<string> Authenticate()
        {
            CleanupHeaders();
            var data = await Query(Constants.LOGIN_ENDPOINT, HttpMethod.Post);

            var results = ResultResponse.FromJson(data);
            if (results.Success)
            {
                var details = LoginSuccess.FromJson(data);
                authInfo = details.Data;
                return null;
            } else
            {
                var details = ResultFailure.FromJson(data);
                return details.Data.Message;
            }
        }

        private void CleanupHeaders()
        {
            headers.Clear();
            headers.Add("Accept", "application/json");
            headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.84 Safari/537.36");
            if (authInfo != null)
            {
                headers.Add("Authorization", authInfo.Token);
            }

            parameters.Clear();
            parameters.Add("email", authUsername);
            parameters.Add("password", authPassword);
        }

        public async Task<string> Query(string url,
              HttpMethod method,
              Dictionary<string,string> extra_params= null,
              Dictionary<string,string> extra_headers = null,
              int retry= 3,
              bool raw = false,
              bool stream = false)
        {
            CleanupHeaders();

            var client = new HttpClient();

            var request = new HttpRequestMessage(method, url);

            headers.ToList().ForEach(x => request.Headers.Add(x.Key, x.Value));

            if (extra_headers != null && extra_headers.Count > 0)
            {
                extra_headers.ToList().ForEach(x => request.Headers.Add(x.Key, x.Value));
            }
            Dictionary<string, string> instanceParameters = new Dictionary<string,string>(parameters);
            if (extra_params != null && extra_params.Count > 0)
            {
                extra_params.ToList().ForEach(x => instanceParameters[x.Key] = x.Value);
            }
            switch (method.Method)
            {
                case "GET":
                    break;
                case "PUT":
                case "POST":
                    string json = JsonConvert.SerializeObject(instanceParameters);
                    Debug.WriteLine(json);
                    request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    break;
            }
            string resultJSON = null;
            int attempt = 1;
            Exception lastException = null;
            bool success = false;
            while ((attempt <= retry) && (!success))
            {
                try
                {
                    lastException = null;
                    var response = await client.SendAsync(request);
                    resultJSON = await response.Content.ReadAsStringAsync();
                    success = true;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    attempt++;
                    success = false;
                }
            }
            if (lastException != null)
            {
                throw lastException;
            }
            return resultJSON; 
        }
 
    }
}
