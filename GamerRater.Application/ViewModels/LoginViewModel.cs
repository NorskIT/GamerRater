using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GamerRater.Application.Annotations;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;
using User = GamerRater.Model.User;

namespace GamerRater.Application.ViewModels
{
    public class LoginViewModel: INotifyPropertyChanged
    {
        public ICommand RegisterUserCommand => new RelayCommand(() => NavigationService.Navigate<RegistrationPage>());
        public ICommand LoginUserCommand;
        public ICommand CancelCommand => new RelayCommand(() => NavigationService.GoBack());
        public User registeredUser;
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
                    var existingUser = await new UserAuthenticator().LogInUser(user);
                    if (existingUser != null) { 
                        if(registeredUser != null)
                        {
                            NavigationService.Navigate<MainPage>();
                            NavigationService.Frame.BackStack.Clear();
                            return;
                        }
                        NavigationService.GoBack();
                    }
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
