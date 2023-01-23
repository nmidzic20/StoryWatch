using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IGDB;
using IGDB.Models;
using System.Web.UI;
using TMDbLib.Objects.Search;

namespace BusinessLayer
{
    public class GameServices
    {
        private readonly string id = "ba8wwb974gpp9d1azk47t4g7mzibg5";
        private readonly string secret = "toc5evt7on3gbpkr6iwtolt7swobvi";

        private IGDBClient api;

        public GameServices()
        {
            api = new IGDBClient(id, secret);
        }

        public async Task<IGDB.Models.Game[]> SearchGamesAsync(string name)
        {
            return await api.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query: $"fields name, id, summary; search \"{name}\";");
        }
    }
}
