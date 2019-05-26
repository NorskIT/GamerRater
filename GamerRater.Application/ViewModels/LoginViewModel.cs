using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using GamerRater.Application.Annotations;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using GamerRater.Model;

namespace GamerRater.Application.ViewModels
{
    public class LoginViewModel: INotifyPropertyChanged
    {
        public ICommand RegisterUserCommand => new RelayCommand(() => NavigationService.Navigate<RegistrationPage>());
        public ICommand LoginUserCommand;
        public ICommand CancelCommand => new RelayCommand(() => NavigationService.GoBack());
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _errorLogin;
        private bool _awaitLogin;
        public bool ErrorLogin {
            get => _errorLogin;
            set { _errorLogin = value; OnPropertyChanged(); }
        }
        public bool AwaitLogin
        {
            get => _awaitLogin;
            set { _awaitLogin = value; OnPropertyChanged("AwaitLogin"); }
        }
        
        public void Initialize()
        {
            AwaitLogin = true;
            LoginUserCommand =
                new RelayCommand<User>(async user =>
                {
                    AwaitLogin = false;
                    User existingUser = await new UserAuthenticator().LogInUser(user);
                    if (existingUser != null)
                        NavigationService.GoBack();
                    else
                        ErrorLogin = true;
                    AwaitLogin = true;
                }, user => AwaitLogin);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
