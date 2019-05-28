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
        public static UserAuthenticator SessionUserAuthenticator = new UserAuthenticator();
        

        private User _user;

        public User User
        {
            get => _user;
            set => Set(ref _user, value);
        }
        
        private static bool _userLoggedInBool;
        public bool UserLoggedInBool
        {
            get => _userLoggedInBool;
            set => Set(ref _userLoggedInBool, value);
        }

        public async Task<User> LogInUser(User user)
        {
            var existingUser = await new Users().GetUser(user.Username);
            if (existingUser == null) return null;
            if (!existingUser.Password.Equals(user.Password)) return null;
            var completeUser = await new Users().GetUser(existingUser.Id);
            User = completeUser;
            UserLoggedInBool = true;
            SessionUserAuthenticator = this;
            await UpdateUser();
            return existingUser;
        }

        public async Task<bool> UpdateUser()
        {
            try
            {
                var completeUser = await new Users().GetUser(User.Id);
                User.Reviews = completeUser.Reviews;
                return true;
            }
            catch (Exception ex)
            {
                //TODO:HANDLE UPDATE ERROR
                return false;
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
