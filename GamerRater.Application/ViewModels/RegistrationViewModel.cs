using System.Net.Http;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using GamerRater.Model;

namespace GamerRater.Application.ViewModels
{
    public class RegistrationViewModel : Observable
    {
        public enum RegistrationError
        {
            UsernameAlreadyInUse,
            NetworkError,
            IllegalValues,
            None
        }

        private Button _button;
        private User _newUser;

        public RegistrationViewModel()
        {
            RegisterUserCommand =
                new RelayCommand<User>(async user =>
                {
                    Page.ErrorMessage(RegistrationError.None);
                    if (Users.UserDataValidator(user))
                    {
                        using (var users = new Users())
                        {
                            try
                            {
                                if (await users.GetUser(user.Username) == null)
                                {
                                    if (await users.AddUser(user))
                                    {
                                        Window.Current.CoreWindow.PointerCursor =
                                            new CoreCursor(CoreCursorType.Arrow, 0);
                                        NavigationService.Navigate<LoginPage>(user);
                                        return;
                                    }

                                    Page.ErrorMessage(RegistrationError.NetworkError);
                                    return;
                                }
                            }
                            catch (HttpRequestException)
                            {
                                Page.ErrorMessage(RegistrationError.NetworkError);
                                return;
                            }
                        }

                        Page.ErrorMessage(RegistrationError.UsernameAlreadyInUse);
                        return;
                    }

                    Page.ErrorMessage(RegistrationError.IllegalValues);
                }, SetUser);
        }

        public RegistrationPage Page { get; set; }
        public ICommand RegisterUserCommand { get; set; }
        public ICommand CancelCommand => new RelayCommand(() => NavigationService.GoBack());


        /// <summary>Sets the button which is used up against CanRegister()</summary>
        /// <param name="button">The button.</param>
        public void SetButton(Button button)
        {
            _button = button;
            button.IsEnabled = false;
        }

        /// <summary> Refreshed user object recieved from relay command</summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public bool SetUser(User user)
        {
            _newUser = user;
            return true;
        }


        /// <summary>Determines whether this instance can register.</summary>
        public void CanRegister()
        {
            _button.IsEnabled = !string.IsNullOrWhiteSpace(_newUser.Email) &&
                                !string.IsNullOrWhiteSpace(_newUser.Password) &&
                                !string.IsNullOrWhiteSpace(_newUser.LastName) &&
                                !string.IsNullOrWhiteSpace(_newUser.FirstName) &&
                                !string.IsNullOrWhiteSpace(_newUser.Username) &&
                                _newUser.Email.Length > 2 &&
                                _newUser.Password.Length > 2 &&
                                _newUser.LastName.Length > 2 &&
                                _newUser.FirstName.Length > 2 &&
                                _newUser.Username.Length > 2;
        }
    }
}
