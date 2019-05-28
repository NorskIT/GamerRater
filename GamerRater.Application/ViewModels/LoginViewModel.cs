using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using User = GamerRater.Model.User;

namespace GamerRater.Application.ViewModels
{
    public class LoginViewModel: INotifyPropertyChanged
    {
        public ICommand RegisterUserCommand => new RelayCommand(() => NavigationService.Navigate<RegistrationPage>());
        public ICommand LoginUserCommand;
        public ICommand CancelCommand => new RelayCommand(() => NavigationService.GoBack());
        public LoginPage Page;
        public User registeredUser;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _errorLogin;
        public bool ErrorLogin {
            get => _errorLogin;
            set { _errorLogin = value; OnPropertyChanged(); }
        }
        
        public void Initialize()
        {
            LoginUserCommand =
                new RelayCommand<User>(async user =>
                {
                    Page.VisualWait(false);
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
                    Page.VisualWait(true);
                });
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
