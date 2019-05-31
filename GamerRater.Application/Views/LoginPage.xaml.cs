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

        /// <summary>Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// If user is not null, will preset login info with user info</summary>
        /// <param name="e">
        /// Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.
        /// </param>
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

        /// <summary>Disable textfield and button to visualize wait-mode</summary>
        /// <param name="wait">if set to <c>true</c> [wait].</param>
        public void AwaitLogin(bool wait)
        {
            Username.IsEnabled = !wait;
            Password.IsEnabled = !wait;
            RegistrationButton.IsEnabled = !wait;
            CancelButton.IsEnabled = !wait;
            LoginButton.IsEnabled = !wait;
            Window.Current.CoreWindow.PointerCursor = wait ? new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 0) : new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
        }

        /// <summary>Present user with error received</summary>
        /// <param name="error">The error.</param>
        /// <exception cref="ArgumentOutOfRangeException">error - null</exception>
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
