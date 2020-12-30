using Sentry.Protocol;
using Sample.Xamarin.Core.Extensions;
using Sample.Xamarin.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Sentry;

namespace Sample.Xamarin.Core.ViewModels.Popups
{
    public partial class UserFeedbackPopupPageViewModel : NavigationService
    {
        private SentryId _id;
        private string _errorShortId;
        public string ErrorShortId 
        {
            get => _errorShortId;
            set
            {
                _errorShortId = value;
                RaisePropertyChanged(() => ErrorShortId);
            } 
        }
        private string _username;
        public string Username 
        {
            get => _username;
            set
            {
                _username = value;
                RaisePropertyChanged(() => Username);
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }
        private bool _emailInvalid = true;
        public bool EmailInvalid
        {
            get => _emailInvalid;
            set
            {
                _emailInvalid = value;
                RaisePropertyChanged(() => EmailInvalid);
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }
        private bool _descriptionInvalid = true;
        public bool DescriptionInvalid
        {
            get => _descriptionInvalid;
            set
            {
                _descriptionInvalid = value;
                RaisePropertyChanged(() => DescriptionInvalid);
            }
        }
        #region Commands

        public Command EmailChangedCmd { get; }
        public Command DescriptionChangedCmd { get; }
        public Command SendFeedbackCmd { get; }
        public Command CancelCmd { get; }


        #endregion

        public UserFeedbackPopupPageViewModel()
        {
            EmailChangedCmd = new Command(EmailChanged);
            DescriptionChangedCmd = new Command(DescriptionChanged);
            CancelCmd = new Command(Close);
            SendFeedbackCmd = new Command(Send);
        }
        #region Action

        private Action Close => () =>
        {
            _ = ClosePopup();
        };

        private Action Send => () =>
        {
            if (Validate())
            {
               // SentrySdk.CaptureUserFeedback(_id, Email, Description, Username);
                _ = ClosePopup();
            }
        };

        private Action EmailChanged => () =>
        {
            ValidateEmail();
        };

        private Action DescriptionChanged => () =>
        {
            ValidateDescription();
        };

        #endregion

        #region Data Manipulation
        private bool Validate()
            => ValidateEmail() && ValidateDescription();

        private bool ValidateEmail()
        {
            if (Email == null)
            {
                EmailInvalid = true;
                return false;
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(Email);

            EmailInvalid = !match.Success;
            return match.Success;
        }

        private bool ValidateDescription()
        {
            DescriptionInvalid = string.IsNullOrWhiteSpace(Description);
            return !DescriptionInvalid;
        }


        public override void Initialize(Dictionary<string, object> parameters)
        {
            if (parameters["SentryId"] is SentryId id)
            {
                _id = id;
                ErrorShortId = _id.GetShortId();
                Validate();
            }
        }
        #endregion
    }
}
