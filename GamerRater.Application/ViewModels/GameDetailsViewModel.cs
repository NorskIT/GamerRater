using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamerRater.Model.IGDBModels;

namespace GamerRater.Application.ViewModels
{
    public class GameDetailsViewModel
    {
        public GameRoot mainGame;

        public void Initialize(GameRoot game)
        {
            mainGame = game;
        }
    }


}
