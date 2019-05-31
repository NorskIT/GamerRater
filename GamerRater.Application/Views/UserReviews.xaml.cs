using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GamerRater.Application.ViewModels;

namespace GamerRater.Application.Views
{
    
    public sealed partial class UserReviews : Page
    {
        UserReviewsViewModel _viewModel = new UserReviewsViewModel();
        public UserReviews()
        {
            this.InitializeComponent();
            _viewModel.Initialize();
            _viewModel.Page = this;
        }
    }
}
