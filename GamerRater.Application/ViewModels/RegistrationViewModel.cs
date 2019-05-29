using System.ComponentModel;
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
        private Visibility _faultyData = Visibility.Collapsed;
        public Visibility FaultyData
        {
            get => _faultyData;
            set => Set(ref _faultyData, value);
        }
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
                    button.IsEnabled = false;
                    if(Users.UserDataValidator(user)) {
                        if(await new Users().GetUser(user.Username) == null)
                        {
                            if (await new Users().AddUser(user)) {
                                NavigationService.Navigate<LoginPage>(user);
                                return;
                            }
                            Page.ErrorInfo(RegistrationError.NetworkError);
                            return;
                        }
                        Page.ErrorInfo(RegistrationError.UsernameAlreadyInUse);
                        return;
                    }
                    Page.ErrorInfo(RegistrationError.IllegalValues);
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
