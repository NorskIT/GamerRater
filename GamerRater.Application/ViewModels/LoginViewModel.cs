using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using Newtonsoft.Json;
using User = GamerRater.Model.User;

namespace GamerRater.Application.ViewModels
{
    public class LoginViewModel
    {
        public enum LoginError
        {
            WrongUsernameOrPassword,
            NetworkError,
            None
        }

        public ICommand RegisterUserCommand => new RelayCommand(() => NavigationService.Navigate<RegistrationPage>());
        public ICommand LoginUserCommand;
        public ICommand CancelCommand => new RelayCommand(() => NavigationService.GoBack());
        public LoginPage Page;
        //Flag: Has user just registered?
        public User RegisteredUser;
        
        public void Initialize()
        {
            LoginUserCommand =
                new RelayCommand<User>(async user =>
                {
                    Page.AwaitLogin(true);
                    Page.ErrorMessage(LoginError.None);
                    try
                    {
                        try
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
                                Page.ErrorMessage(LoginError.WrongUsernameOrPassword);
                        }
                        catch (JsonException)
                        {
                            Page.ErrorMessage(LoginError.NetworkError);
                        }
                    }
                    catch (HttpRequestException)
                    {
                        Page.ErrorMessage(LoginError.NetworkError);
                    }
                    Page.AwaitLogin(false);
                });
        }
    }
}
