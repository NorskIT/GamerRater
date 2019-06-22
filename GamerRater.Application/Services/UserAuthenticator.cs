using System;
using System.Threading.Tasks;
using GamerRater.Application.DataAccess;
using GamerRater.Application.Helpers;
using GamerRater.Model;

namespace GamerRater.Application.Services
{
    public class UserAuthenticator : Observable
    {
        private static bool _userLoggedInBool;


        private User _user;


        static UserAuthenticator()
        {
            SessionUserAuthenticator = new UserAuthenticator();
        }

        public static UserAuthenticator SessionUserAuthenticator { get; set; }

        public User User
        {
            get => _user;
            set => Set(ref _user, value);
        }

        public bool UserLoggedInBool
        {
            get => _userLoggedInBool;
            set => Set(ref _userLoggedInBool, value);
        }

        /// <summary>  Simulation a login of user and creates a "session" available to the rest of the app</summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public async Task<User> LogInUser(User user)
        {
            using (var users = new Users())
            {
                var existingUser = await users.GetUser(user.Username).ConfigureAwait(true);
                if (existingUser == null) return null;
                if (!existingUser.Password.Equals(user.Password, StringComparison.CurrentCulture)) return null;
                var contextUser = await users.GetUser(existingUser.Id).ConfigureAwait(true);
                if (contextUser == null)
                {
                    GrToast.SmallToast(GrToast.Errors.NetworkError);
                    return null;
                }

                var completeUser = contextUser;
                User = completeUser;
                UserLoggedInBool = true;
                SessionUserAuthenticator = this;
                if (!await UpdateUser().ConfigureAwait(true)) return null;
                return existingUser;
            }
        }

        /// <summary>Synchronise user to database</summary>
        /// <returns></returns>
        public async Task<bool> UpdateUser()
        {
            using (var users = new Users())
            {
                var user = await users.GetUser(User.Id).ConfigureAwait(true);
                if (user == null)
                {
                    GrToast.SmallToast(GrToast.Errors.NetworkError);
                    return false;
                }

                var completeUser = user;
                User.Reviews = completeUser.Reviews;
                return true;
            }
        }


        /// <summary>  Simulate a logout</summary>
        /// <returns></returns>
        public bool LogOut()
        {
            User = null;
            UserLoggedInBool = false;
            return true;
        }
    }
}
