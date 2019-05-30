using System;
using System.Net.NetworkInformation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GamerRater.Application.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GamerRater.Application.Views
{
    /// <inheritdoc />
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegistrationPage : Page
    {
        private readonly RegistrationViewModel ViewModel = new RegistrationViewModel();

        public RegistrationPage()
        {
            InitializeComponent();
            ViewModel.Page = this;
            ViewModel.SetButton(RegisterButton);
        }

        public void ErrorMessage(RegistrationViewModel.RegistrationError error)
        {
            WaitVisual(true);
            ErrorInfoTextBlock.Visibility = Visibility.Visible;
            switch (error)
            {
                case RegistrationViewModel.RegistrationError.IllegalValues:
                    ErrorInfoTextBlock.Text = "* Fields cannot contain symbols or spaces!";
                    break;
                case RegistrationViewModel.RegistrationError.NetworkError:
                    ErrorInfoTextBlock.Text = !NetworkInterface.GetIsNetworkAvailable() ? "* Could not connect to server. Check your network connection and try again" : "* API not responding. Please try again later.";
                    break;
                case RegistrationViewModel.RegistrationError.UsernameAlreadyInUse:
                    ErrorInfoTextBlock.Text = "* Username already in use..";
                    break;
                case RegistrationViewModel.RegistrationError.None:
                    ErrorInfoTextBlock.Text = "";
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
            WaitVisual(false);
        }

        private void WaitVisual(bool wait)
        {
            Window.Current.CoreWindow.PointerCursor = wait ? new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 0) : new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            RegisterButton.IsEnabled = !wait;
            CancelButton.IsEnabled = !wait;
            Username.IsEnabled = !wait;
            Password.IsEnabled = !wait;
            Mail.IsEnabled = !wait;
            Firstname.IsEnabled = !wait;
            Lastname.IsEnabled = !wait;
        }
    }
}
