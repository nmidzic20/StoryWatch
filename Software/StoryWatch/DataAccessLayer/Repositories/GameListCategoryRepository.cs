using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Function: database communication for game list categories
    /// Author: Hrvoje Lukšić
    /// </summary>
    public class GameListCategoryRepository : Repository<GameListCategory>
    {
        public void DeleteListForUser(GameListCategory gameListCategory, User loggedUser)
        {
            var gameRepo = new GameRepository();
            var gamesOnThisList = gameRepo.GetGamesForList(gameListCategory, loggedUser);
            foreach (var game in gamesOnThisList)
            {
                //since games are deleted with the list
                //check if any list for any user, still references these games
                //if not, no sense in keeping the game in DB - delete it as well
                if (Context.GameListItems.Count(g => g.Id_Games == game.Id) == 1)
                {
                    var unusedGame = Context.Games.SingleOrDefault(g => g.Id == game.Id);
                    Context.Games.Remove(unusedGame);
                    SaveChanges();
                }
            }
            Context.Users.Attach(loggedUser);
            var gameList = loggedUser.GameListCategories
                .FirstOrDefault(gl => gl.Id == gameListCategory.Id);

            loggedUser.GameListCategories.Remove(gameList);
            SaveChanges();
        }

        public int UpdateListForUser(GameListCategory entity, User loggedUser, bool saveChanges = true)
        {
            var gameListCategory = Entities.SingleOrDefault(e => e.Id == entity.Id);
            gameListCategory.Title = entity.Title;
            gameListCategory.Color = entity.Color;

            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public IQueryable<GameListCategory> GetGameListCategories()
        {
            var query = from l in Entities
                        select l;

            return query;
        }
    }
}
