using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GamerRater.Application.Annotations;
using GamerRater.Application.DataAccess;
using GamerRater.Model;

namespace GamerRater.Application.Services
{
    public class UserAuthenticator : INotifyPropertyChanged
    {
        public static UserAuthenticator SessionUserAuthenticator = new UserAuthenticator();

        private static bool _userLoggedIn;

        public static User LoggedInUser { get; set; }

        public bool UserLoggedIn
        {
            get => _userLoggedIn;
            set
            {
                _userLoggedIn = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<User> LogInUser(User user)
        {
            var existingUser = await new Users().GetUser(user.Username);
            if (existingUser == null) return null;
            if (!existingUser.Password.Equals(user.Password)) return null;
            LoggedInUser = existingUser;
            UserLoggedIn = true;
            SessionUserAuthenticator = this;
            return existingUser;
        }

        //Simple log out method
        public bool LogOut()
        {
            LoggedInUser = null;
            UserLoggedIn = false;
            return true;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
