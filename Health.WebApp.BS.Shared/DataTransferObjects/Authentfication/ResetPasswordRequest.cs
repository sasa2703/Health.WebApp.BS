using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthManager.WebApp.BS.Shared.DataTransferObjects.Authentfication
{
    public class ResetPasswordRequest
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("connection")]
        public string Connection { get; set; }
    }
}
