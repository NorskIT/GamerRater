using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerRater.Application.DataAccess
{
    //TODO: WHat is this?
    static class BaseUri
    {
        private static readonly string RootUrl = "http://localhost:61971/api/";
        private static readonly string IGDBUrl = "https://api-v3.igdb.com/";
        public static readonly string Games = RootUrl + "GameRoots/";
        public static readonly string Covers = RootUrl + "Covers/";
        public static readonly string Users = RootUrl + "Users/";
        public static readonly string Username = RootUrl + "Users/Username/";
        public static readonly string Reviews = RootUrl + "Reviews/";
        public static readonly string UserGroups = RootUrl + "UserGroups/";
        public static readonly string UserGroupName = RootUrl + "UserGroups/Group/";
        public static readonly string IGDBGames = IGDBUrl + "Games/";
        public static readonly string IGDBRatings = IGDBUrl + "Ratings/";
        public static readonly string IGDBPlatforms = IGDBUrl + "Platforms/";
        public static readonly string IGDBCovers = IGDBUrl + "Covers/";
    }
}
