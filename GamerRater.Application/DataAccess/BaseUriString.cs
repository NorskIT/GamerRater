namespace GamerRater.Application.DataAccess
{
    /// <summary>A class with a collection of common used Uri's within the application</summary>
    internal static class BaseUriString
    {
        /// <summary>Uri pointing towards the API</summary>
        public static readonly string Port = "61971"; // CHANGE THIS EQUAL TO API PORT

        public static readonly string RootUrl = "http://localhost:" + Port + "/api/";
        public static readonly string IGDBUrl = "https://api-v3.igdb.com/";
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
