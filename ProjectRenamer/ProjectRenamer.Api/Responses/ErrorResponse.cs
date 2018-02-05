using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRenamer.Api.Responses
{
    public class ErrorResponse
    {
        public string FriendlyMessage { get; private set; }

        public void AddErrorMessage(string message)
        {
            FriendlyMessage = string.IsNullOrEmpty(FriendlyMessage) ? message : $"{FriendlyMessage}{Environment.NewLine}{message}";
        }
    }
}