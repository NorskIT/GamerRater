using System;

using GamerRater.Application.Services;

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using GamerRater.Application.DataAccess;
using GamerRater.Model;

namespace GamerRater.Application
{
    public sealed partial class App : Windows.UI.Xaml.Application
    {
        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {

            InitializeComponent();
            InitializeAppRequirements();

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        private async void InitializeAppRequirements()
        {
            var userGroup = await new UserGroups().GetUserGroup(1);
            if (userGroup == null)
            {
                try
                {
                    await new UserGroups().AddUserGroup(new UserGroup() {Group = "User"});
                    await new UserGroups().AddUserGroup(new UserGroup() {Group = "Admin"});
                }
                catch (Exception ex)
                {
                    //TODO: NO INTERNET
                }
            }
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(Views.MainPage), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            return new Views.ShellPage();
        }
    }
}
