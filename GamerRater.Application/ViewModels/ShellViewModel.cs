using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using GamerRater.Application.Helpers;
using GamerRater.Application.Services;
using GamerRater.Application.Views;
using User = GamerRater.Model.User;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace GamerRater.Application.ViewModels
{
    public class ShellViewModel : Observable
    {
        private readonly KeyboardAccelerator _altLeftKeyboardAccelerator =
            BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);

        private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);
        private bool _isBackEnabled;
        private ICommand _itemInvokedCommand;
        private IList<KeyboardAccelerator> _keyboardAccelerators;
        private ICommand _loadedCommand;
        public User _loggedInUser;
        private WinUI.NavigationView _navigationView;
        private bool _notOnRegistrationLoginPage;
        private WinUI.NavigationViewItem _selected;
        private UserAuthenticator _session;

        public UserAuthenticator Session
        {
            get => UserAuthenticator.SessionUserAuthenticator;
            set
            {
                _session = value;
                LoggedInUser = value.User;
            }
        }

        public ICommand GoToLoginPage => new RelayCommand(() => NavigationService.Navigate<LoginPage>());

        public ICommand LogOutCommand => new RelayCommand(() =>
        {
            UserAuthenticator.SessionUserAuthenticator.LogOut();
            NavigationService.Navigate<LoginPage>();
            GrToast.SmallToast("Logged out");
        });

        public ShellPage Page { get; set; }

        public bool IsBackEnabled
        {
            get => _isBackEnabled;
            set => Set(ref _isBackEnabled, value);
        }

        public WinUI.NavigationViewItem Selected
        {
            get => _selected;
            set => Set(ref _selected, value);
        }

        public User LoggedInUser
        {
            get => _loggedInUser;
            set => Set(ref _loggedInUser, value);
        }

        public bool NotOnRegistrationLoginPage
        {
            get => _notOnRegistrationLoginPage;
            set => Set(ref _notOnRegistrationLoginPage, value);
        }

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));

        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand =
                                                  new RelayCommand<WinUI.NavigationViewItemInvokedEventArgs>(
                                                      OnItemInvoked));

        public void Initialize(Frame frame, WinUI.NavigationView navigationView,
            IList<KeyboardAccelerator> keyboardAccelerators)
        {
            _navigationView = navigationView;
            _keyboardAccelerators = keyboardAccelerators;
            NavigationService.Frame = frame;
            NavigationService.NavigationFailed += (sender, e) => throw e.Exception;
            NavigationService.Navigated += Frame_Navigated;
            _navigationView.BackRequested += OnBackRequested;
        }

        private void OnLoaded()
        {
            // Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
            // More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
            _keyboardAccelerators.Add(_altLeftKeyboardAccelerator);
            _keyboardAccelerators.Add(_backKeyboardAccelerator);
        }

        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            var item = _navigationView.MenuItems
                .OfType<WinUI.NavigationViewItem>()
                .First(menuItem => (string) menuItem.Content == (string) args.InvokedItem);
            var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
            NavigationService.Navigate(pageType);
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            var frame = (Frame) sender;
            //Check if next page is on RegistrationPage/LoginPage so that Login-button can be hidden
            NotOnRegistrationLoginPage = !(frame.Content is RegistrationPage || frame.Content is LoginPage);
            Session = UserAuthenticator.SessionUserAuthenticator;
            IsBackEnabled = NavigationService.CanGoBack;
            Selected = _navigationView.MenuItems
                .OfType<WinUI.NavigationViewItem>()
                .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
            return pageType == sourcePageType;
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key,
            VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator {Key = key};
            if (modifiers.HasValue) keyboardAccelerator.Modifiers = modifiers.Value;

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender,
            KeyboardAcceleratorInvokedEventArgs args)
        {
            var result = NavigationService.GoBack();
            args.Handled = result;
        }
    }
}
