using System;
using System.Linq.Expressions;
using System.Net;
using GamerRater.Application.Services;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using GamerRater.Application.DataAccess;
using GamerRater.Model;
using Newtonsoft.Json;

namespace GamerRater.Application
{
    public sealed partial class App : Windows.UI.Xaml.Application
    {
        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService => _activationService.Value;

        public App()
        {

            InitializeComponent();

            /*************************************
             *      Uncomment method below       *
             *      if you need to setup         *
             *           the database            *
             *************************************/
            //InitializeAppRequirements();

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.

            
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        
        private static async void InitializeAppRequirements()
        {
            
            using (var userGroups = new UserGroups())
            {
                var userGroup = await userGroups.GetUserGroup("User").ConfigureAwait(false);
                if (userGroup != null) return;
                if (await userGroups.AddUserGroup(new UserGroup() {Group = "User"}).ConfigureAwait(false) !=
                    HttpStatusCode.Created)
                {
                    GrToast.SmallToast(GrToast.Errors.NetworkError);
                    return;
                }

                if (await userGroups.AddUserGroup(new UserGroup() {Group = "Admin"}).ConfigureAwait(false) !=
                    HttpStatusCode.Created)
                {
                    GrToast.SmallToast(GrToast.Errors.NetworkError);
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
