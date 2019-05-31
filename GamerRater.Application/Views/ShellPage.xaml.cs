using Windows.UI.Xaml.Controls;
using GamerRater.Application.ViewModels;

namespace GamerRater.Application.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellPage()
        {
            InitializeComponent();
            ViewModel.Page = this;
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView, KeyboardAccelerators);
        }

        public ShellViewModel ViewModel { get; } = new ShellViewModel();
    }
}
