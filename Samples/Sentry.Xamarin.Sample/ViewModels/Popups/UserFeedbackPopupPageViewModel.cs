using Sentry.Xamarin.Sample.Interfaces;
using Sentry.Xamarin.Sample.Rules;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xamarin.Forms;

namespace Sentry.Xamarin.Sample.ViewModels.Popups
{
    public partial class UserFeedbackPopupPageViewModel : ApplicationBridge
    {
        public string Username { get; set; }

        private ValidatableObject<string> _email;
        public ValidatableObject<string> Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }
        private ValidatableObject<string> _description;
        public ValidatableObject<string> Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }
        #region Commands

        public Command EmailChangedCmd { get; }
        public Command DescriptionChangedCmd { get; }
        public Command SendFeedbackCmd { get; }
        public Command CloseCmd { get; }


        #endregion

        public UserFeedbackPopupPageViewModel()
        {
            _email = new ValidatableObject<string>(); 
            _email.Validations.Add(new EmailRule<string>
            {
                ValidationMessage = "A valid email is required."
            });
            _description = new ValidatableObject<string>();
            _description.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "A description is required."
            });
            EmailChangedCmd = new Command(EmailChanged);
            DescriptionChangedCmd = new Command(DescriptionChanged);
        }
        #region Action

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
            => _email.Validate();

        private bool ValidateDescription()
            => _description.Validate();

        #endregion
    }
}
