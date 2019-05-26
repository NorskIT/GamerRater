using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerRater.Application.DataAccess
{
    static class BaseUri
    {
        private static readonly string RootUrl = "http://localhost:61971/api/";
        public static readonly string Games = RootUrl + "GameRoots/";
        public static readonly string Covers = RootUrl + "Covers/";
        public static readonly string Users = RootUrl + "Users/";
        public static readonly string Username = RootUrl + "Users/Username/";
        public static readonly string Reviews = RootUrl + "Reviews/";
        public static readonly string UserGroups = RootUrl + "UserGroups/";
    }
}
