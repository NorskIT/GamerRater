﻿using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using GamerRater.Application.Services;

namespace GamerRater.Application.Activation
{
    internal class DefaultLaunchActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
    {
        private readonly Type _navElement;

        public DefaultLaunchActivationHandler(Type navElement)
        {
            _navElement = navElement;
        }

        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            // When the navigation stack isn't restored, navigate to the first page and configure
            // the new page by passing required information in the navigation parameter
            NavigationService.Navigate(_navElement, args.Arguments);

            await Task.CompletedTask.ConfigureAwait(true);
        }

        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            // None of the ActivationHandlers has handled the app activation
            return NavigationService.Frame.Content == null;
        }
    }
}
