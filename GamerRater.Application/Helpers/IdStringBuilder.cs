using System.Collections.Generic;
using GamerRater.Model;

namespace GamerRater.Application.Helpers
{
    /// <summary>String builder made for converting IGDB objects into ID strings</summary>
    public static class IdStringBuilder
    {
        /// <summary>Builds a Cover ID string compatible with the api query. Etc : (123, 432, 12994, 392).</summary>
        /// <param name="games">The games.</param>
        /// <returns></returns>
        public static string GameIds(IEnumerable<GameRoot> games)
        {
            var ids = "";
            var firstIterate = true;
            foreach (var game in games)
            {
                if (firstIterate)
                    ids += game.Cover;
                else
                    ids += ", " + game.Cover;
                firstIterate = false;
            }

            return ids;
        }

        /// <summary>Builds a Platform ID string compatible with the api query. Etc : (123, 432, 12994, 392)</summary>
        /// <param name="games">The games to fetch platform ids from</param>
        /// <returns></returns>
        public static string PlatformIds(GameRoot game)
        {
            var ids = "";
            var firstIterate = true;
            foreach (var id in game.PlatformsIds)
            {
                if (firstIterate)
                    ids += id;
                else
                    ids += ", " + id;
                firstIterate = false;
            }

            return ids;
        }
    }
}
