using System;
using Windows.UI.Xaml;
using GamerRater.Application.ViewModels;

using Windows.UI.Xaml.Controls;

namespace GamerRater.Application.Views
{
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
