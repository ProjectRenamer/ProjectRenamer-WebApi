using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRenamer.Api.Responses
{
    public class ErrorResponse
    {
        public string FriendlyMessage { get; private set; }
        public List<Exception> ExceptionsDetail { get; private set; } = new List<Exception>();

        public void AddErrorMessage(string message, Exception ex = null)
        {
            FriendlyMessage = string.IsNullOrEmpty(FriendlyMessage) ? message : $"{FriendlyMessage}{Environment.NewLine}{message}";
            if (ex != null)
            {
                ExceptionsDetail.Add(ex);
            }
        }
    }
}