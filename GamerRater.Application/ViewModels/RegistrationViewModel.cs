using System.ComponentModel;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using GamerRater.Model;

namespace GamerRater.Application.ViewModels
{
    internal class RegistrationViewModel : Observable, INotifyPropertyChanged
    {
        private Button button;
        private User newUser;

        public RegistrationViewModel()
        {
            RegisterUserCommand =
                new RelayCommand<User>(async user =>
                {
                    //Forces every registered user to join User group.
                    button.IsEnabled = false;
                    user.UserGroup = await new UserGroups().GetUserGroup(1);
                    if (await new Users().AddUser(user))
                        NavigationService.Navigate<LoginPage>(user);
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

        //RelayCommand refreshes user object everytime its model parameters are changed.
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
