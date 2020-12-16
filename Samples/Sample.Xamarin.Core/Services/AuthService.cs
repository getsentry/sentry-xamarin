using System;

namespace Sample.Xamarin.Core.Services
{
    public class AuthService
    {
        public Tuple<bool, string> Authenticate(string login, string password)
            => login == password && password == "1234" ? new Tuple<bool, string>(true, null) :
            new Tuple<bool, string>(false, "Invalid Login or Password.");

        public void DoLogin(string login, string password)
        {
            ValidatePassword(login, password);
        }

        private bool ValidatePassword(string user, string password)
        {
            var validation = int.Parse(password) / user.Length;
            string notExpected = user;
            return validation != int.Parse(notExpected);
        }
    }
}
