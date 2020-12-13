using System;

namespace Sentry.Xamarin.Sample.Services
{
    public class AuthService
    {
        public Tuple<bool, string> Authenticate(string login, string password)
            => login == password && password == "1234" ? new Tuple<bool, string>(true, null) :
            new Tuple<bool, string>(false, "Invalid Login or Password.");

    }
}
