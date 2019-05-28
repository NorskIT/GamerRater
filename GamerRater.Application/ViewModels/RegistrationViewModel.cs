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
    internal class RegistrationViewModel : Observable
    {
        private Button button;
        private User newUser;
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
                        if(await new Users().GetUser(user.Username) == null) { 
                            if (await new Users().AddUser(user)) {
                                NavigationService.Navigate<LoginPage>(user);
                            }
                            //TODO: NO INTERNET
                        }
                        UserAlreadyRegistered = Visibility.Visible;
                    } else
                        FaultyData = Visibility.Visible;
                    button.IsEnabled = true;
                }, SetUser);
        }
        
        public ICommand RegisterUserCommand { get; set; }
        public ICommand CancelCommand => new RelayCommand(() => NavigationService.GoBack());

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
                               !string.IsNullOrWhiteSpace(newUser.Username) &&
                               !string.IsNullOrWhiteSpace(newUser.Username);
        }
    }
}
