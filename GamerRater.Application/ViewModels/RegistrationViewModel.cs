using System.ComponentModel;
using System.Net.Http;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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
        private Button button;
        private User newUser;
        public RegistrationPage Page { get; set; }
        public ICommand RegisterUserCommand { get; set; }
        public ICommand CancelCommand => new RelayCommand(() => NavigationService.GoBack());
        //TODO: REmove?
        private Visibility _faultyData = Visibility.Collapsed;
        public Visibility FaultyData
        {
            get => _faultyData;
            set => Set(ref _faultyData, value);
        }
        //TODO: Is in use???
        private Visibility _userAlreadyRegistered = Visibility.Collapsed;
        public Visibility UserAlreadyRegistered
        {
            get => _userAlreadyRegistered;
            set => Set(ref _userAlreadyRegistered, value);
        }

        public RegistrationViewModel()
        {
            RegisterUserCommand =
                new RelayCommand<User>(async user =>
                {
                    Page.ErrorMessage(RegistrationError.None);
                    button.IsEnabled = false;
                    if(Users.UserDataValidator(user)) {
                        using(var users = new Users()) {
                            try
                            {
                                if (await users.GetUser(user.Username) == null)
                                {
                                    if (await users.AddUser(user))
                                    {
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


        public void SetButton(Button button)
        {
            this.button = button;
            button.IsEnabled = false;
        }

        //RelayCommand refreshes user object every time the model parameters are changed.
        public bool SetUser(User user)
        {
            newUser = user;
            return true;
        }

        //TODO: LEss than 16 XAML.
        //Check if all fields are OK
        public void CanRegister()
        {
            button.IsEnabled = !string.IsNullOrWhiteSpace(newUser.Email) &&
                               !string.IsNullOrWhiteSpace(newUser.Password) &&
                               !string.IsNullOrWhiteSpace(newUser.LastName) &&
                               !string.IsNullOrWhiteSpace(newUser.FirstName) &&
                               !string.IsNullOrWhiteSpace(newUser.Username) &&
                               (newUser.Email.Length > 2 && newUser.Email.Length < 16) &&
                               (newUser.Password.Length > 2 && newUser.Password.Length < 16) &&
                               (newUser.LastName.Length > 2 && newUser.LastName.Length < 16) &&
                               (newUser.FirstName.Length > 2 && newUser.FirstName.Length < 16) &&
                               (newUser.Username.Length > 2 && newUser.Username.Length < 16);
        }
    }
}
