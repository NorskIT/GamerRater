using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GamerRater.Application.ViewModels;
using GamerRater.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GamerRater.Application.Views
{


    public sealed partial class LoginPage : Page
    {
        private readonly LoginViewModel _viewModel = new LoginViewModel();

        public LoginPage()
        {
            this.InitializeComponent();
            _viewModel.Initialize();
            _viewModel.Page = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!(e.Parameter is User user)) return;
            _viewModel.RegisteredUser = user;
            Username.Text = user.Username;
            Password.Password = user.Password;
            RegistrationComplete.Text = Username.Text + " successfully registered.";
            RegistrationComplete.Visibility = Visibility.Visible;
        }

        public void AwaitLogin(bool enabled)
        {
            if (enabled)
            {
                CancelButton.IsEnabled = false;
                LoginButton.IsEnabled = false;
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 0);
            }
            else
            {
                CancelButton.IsEnabled = true;
                LoginButton.IsEnabled = true;
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            }
        }

        public void ErrorMessage(LoginViewModel.LoginError error)
        {
            ErrorInfoTextBlock.Visibility = Visibility.Visible;
            RegistrationComplete.Visibility = Visibility.Collapsed;
            switch (error)
            {
                case LoginViewModel.LoginError.NetworkError:
                    ErrorInfoTextBlock.Text = "* Could not connect to server. Check your network connection and try again";
                    break;
                case LoginViewModel.LoginError.WrongUsernameOrPassword:
                    ErrorInfoTextBlock.Text = "* Wrong username/password or user does not exist.";
                    break;
                case LoginViewModel.LoginError.ApiError:
                    ErrorInfoTextBlock.Text = "* API not responding. Please try again later.";
                    break;
                case LoginViewModel.LoginError.None:
                    ErrorInfoTextBlock.Text = "";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
        }
    }
}
