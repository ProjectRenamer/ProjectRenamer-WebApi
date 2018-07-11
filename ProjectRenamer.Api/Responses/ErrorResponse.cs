using System;
using System.Collections.Generic;

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