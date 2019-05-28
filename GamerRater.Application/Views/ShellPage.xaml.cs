﻿using System;
using Windows.UI.Xaml;
using GamerRater.Application.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GamerRater.Application.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();


        public ShellPage()
        {
            InitializeComponent();
            ViewModel.Page = this;
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView, KeyboardAccelerators);
        }
    }
}
