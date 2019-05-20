using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using GamerRater.Application.DataAccess;
using GamerRater.Model;
using Newtonsoft.Json;

namespace GamerRater.Application.ViewModels
{
    public class GameDetailsViewModel
    {
        public GameRoot mainGame;

        public void Initialize(GameRoot game)
        {
            game.GameCover.url = "https://images.igdb.com/igdb/image/upload/t_720p/" + game.GameCover.image_id + ".jpg";
            mainGame = game;
        }
    }


}
