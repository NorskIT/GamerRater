using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GamerRater.Application.ViewModels;

namespace GamerRater.Application.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ViewModel.CheckCache();
            ViewModel.Page = this;
        }

        public MainViewModel ViewModel { get; } = new MainViewModel();

        public void WaitVisual(bool enabled)
        {
            if (enabled)
            {
                GameRootSearchBar.IsEnabled = false;
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Wait, 0);
            }
            else
            {
                GameRootSearchBar.IsEnabled = true;
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
            }
        }
    }
}
