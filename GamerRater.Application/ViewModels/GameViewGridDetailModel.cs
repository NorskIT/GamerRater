using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamerRater.Application.Helpers;
using GamerRater.Model.IGDBModels;

namespace GamerRater.Application.ViewModels
{
    public class GameViewGridDetailModel : Observable
    {
        private GameRoot _game;

        public GameRoot Game
        {
            get { return _game; }
            set { Set(ref _game, value); }
        }

        public GameViewGridDetailModel(GameRoot game)
        {
            _game = game;
        }

        public void Initialize(GameRoot gameRoot)
        {
            /*var data = SampleDataService.GetContentGridData();
            Item = data.First(i => i.OrderId == orderId);*/
        }
    }
}
