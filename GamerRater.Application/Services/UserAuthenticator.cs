using System;
using System.ComponentModel;
using System.Threading.Tasks;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Model;

namespace GamerRater.Application.Services
{
    public class UserAuthenticator : Observable
    {
        public static UserAuthenticator SessionUserAuthenticator { get; set; }
        

        private User _user;

        public User User
        {
            get => _user;
            set => Set(ref _user, value);
        }
        
        private static bool _userLoggedInBool;

        //TODO: Changed, doyble check
        static UserAuthenticator()
        {
            SessionUserAuthenticator = new UserAuthenticator();
        }

        public bool UserLoggedInBool
        {
            get => _userLoggedInBool;
            set => Set(ref _userLoggedInBool, value);
        }

        public async Task<User> LogInUser(User user)
        {
            using(var users = new Users()) { 
                var existingUser = await users.GetUser(user.Username).ConfigureAwait(true);
                if (existingUser == null) return null;
                if (!existingUser.Password.Equals(user.Password, StringComparison.CurrentCulture)) return null;
                var completeUser = await users.GetUser(existingUser.Id).ConfigureAwait(true);
                User = completeUser;
                UserLoggedInBool = true;
                SessionUserAuthenticator = this;
                await UpdateUser().ConfigureAwait(true);
                return existingUser;
            }
        }

        public async Task<bool> UpdateUser()
        {
            using (var users = new Users())
            {
                var completeUser = await users.GetUser(User.Id).ConfigureAwait(true);
                User.Reviews = completeUser.Reviews;
                return true;
            }
        }

        //Simple log out method
        public bool LogOut()
        {
            User = null;
            UserLoggedInBool = false;
            return true;
        }
    }
}
