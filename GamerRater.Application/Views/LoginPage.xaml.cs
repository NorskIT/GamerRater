using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            _viewModel.registeredUser = user;
            Username.Text = user.Username;
            Password.Password = user.Password;
            RegistrationComplete.Text = Username.Text + " successfully registered.";
            RegistrationComplete.Visibility = Visibility.Visible;
        }

        public void VisualWait(bool enabled)
        {
            LoginButton.IsEnabled = enabled;
        }
    }
}
