using System;

namespace DotNet.Template.Dtos.Responses.Token
{
    public class CreateTokenResponse
    {
        public long UserId { get; set; }
        public string TokenValue { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}