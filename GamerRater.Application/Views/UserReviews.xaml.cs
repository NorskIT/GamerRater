using Windows.UI.Xaml.Controls;
using GamerRater.Application.ViewModels;

namespace GamerRater.Application.Views
{
    public sealed partial class UserReviews : Page
    {
        private readonly UserReviewsViewModel _viewModel = new UserReviewsViewModel();

        public UserReviews()
        {
            InitializeComponent();
            _viewModel.Initialize();
            _viewModel.Page = this;
        }
    }
}
