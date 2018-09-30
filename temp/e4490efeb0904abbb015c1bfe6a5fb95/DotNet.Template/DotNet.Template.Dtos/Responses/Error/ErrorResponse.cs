using System;

namespace DotNet.Template.Dtos.Responses.Error
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