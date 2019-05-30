using System.Net.NetworkInformation;
using System.Windows.Input;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using GamerRater.Model;

namespace GamerRater.Application.ViewModels
{
    public class LoginViewModel
    {
        public enum LoginError
        {
            WrongUsernameOrPassword,
            NetworkError,
            None,
            ApiError
        }

        //Flag: Has user just registered?
        public User RegisteredUser { get; set; }
        public LoginPage Page { get; set; }
        public ICommand LoginUserCommand { get; set; }
        public ICommand RegisterUserCommand => new RelayCommand(() => NavigationService.Navigate<RegistrationPage>());
        public ICommand CancelCommand => new RelayCommand(() => NavigationService.GoBack());

        public void Initialize()
        {
            LoginUserCommand =
                new RelayCommand<User>(async user =>
                {
                    Page.AwaitLogin(true);
                    Page.ErrorMessage(LoginError.None);
                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        var existingUser = await new UserAuthenticator().LogInUser(user);
                        if (existingUser != null)
                        {
                            if (RegisteredUser != null)
                            {
                                NavigationService.Navigate<MainPage>();
                                NavigationService.Frame.BackStack.Clear();
                                return;
                            }

                            NavigationService.GoBack();
                        }
                        else
                        {
                            Page.ErrorMessage(LoginError.ApiError);
                        }
                    }
                    else
                    {
                        Page.ErrorMessage(LoginError.NetworkError);
                    }

                    Page.AwaitLogin(false);
                });
        }
    }
}
