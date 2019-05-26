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
            ViewModel.SetButton(RegisterButton);
        }
    }
}
