using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Sample.Xamarin.Core.Query
{
    public class SentryUserFeedback
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }

        public bool Validade()
            => ValidateEmail() && ValidateDescription();

        public bool ValidateEmail()
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(Email);
            return match.Success;
        }

        public bool ValidateDescription()
            => string.IsNullOrWhiteSpace(Description);
    }
}
