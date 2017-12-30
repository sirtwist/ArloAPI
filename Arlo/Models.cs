// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Arlo;
//
//    var data = LoginResponse.FromJson(jsonString);

namespace Arlo
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class LoginResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }


    public partial class LoginSuccess
    {
        [JsonProperty("data")]
        public LoginData Data { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public partial class LoginFailure
    {
        [JsonProperty("data")]
        public LoginError Data { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public partial class LoginError
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    public partial class LoginData
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("paymentId")]
        public string PaymentId { get; set; }

        [JsonProperty("authenticated")]
        public long Authenticated { get; set; }

        [JsonProperty("accountStatus")]
        public string AccountStatus { get; set; }

        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("tocUpdate")]
        public bool TocUpdate { get; set; }

        [JsonProperty("policyUpdate")]
        public bool PolicyUpdate { get; set; }

        [JsonProperty("validEmail")]
        public bool ValidEmail { get; set; }

        [JsonProperty("arlo")]
        public bool Arlo { get; set; }

        [JsonProperty("dateCreated")]
        public long DateCreated { get; set; }
    }

    public partial class LoginSuccess
    {
        public static LoginSuccess FromJson(string json) => JsonConvert.DeserializeObject<LoginSuccess>(json, Converter.Settings);
    }

    public partial class LoginFailure
    {
        public static LoginFailure FromJson(string json) => JsonConvert.DeserializeObject<LoginFailure>(json, Converter.Settings);
    }

    public partial class LoginResponse
    {
        public static LoginResponse FromJson(string json) => JsonConvert.DeserializeObject<LoginResponse>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this LoginSuccess self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
