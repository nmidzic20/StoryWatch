using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GameRepository : Repository<EntitiesLayer.Entities.Game>
    {
        public int AddGameToList(GameListItem gameListItem, bool saveChanges = true)
        {
            Context.GameListItems.Add(gameListItem);
            
            if (saveChanges)
            {
                return Context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int DeleteGameFromList(GameListItem gameListItem, bool saveChanges = true)
        {
            Context.GameListItems.Attach(gameListItem);
            Context.GameListItems.Remove(gameListItem);
            
            if (saveChanges)
            {
                return Context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public IQueryable<Game> GetGameByTitle(string title)
        {
            var query = from m in Entities
                        where m.Title == title
                        select m;

            return query;
        }
        
        public IQueryable<Game> GetGameByIGDBId(string id)
        {
            return from m in Entities
                    where m.IGDB_Id == id
                    select m;
        }

        public IQueryable<Game> GetGameById(int id)
        {
            var query = from m in Entities
                        where m.Id == id
                        select m;

            return query;
        }
        
        public List<Game> GetGamesForList(GameListCategory gameListCategory, User loggedUser)
        {
            //Context.Users.Attach(loggedUser);
            var user = Context.Users.Where(u => u.Id == loggedUser.Id).Single();
            var game_ids = user.GameListItems.Where(l => l.Id_GameListCategories == gameListCategory.Id).Select(l => l.Id_Games).ToList();

            List<Game> games = new List<Game>();

            foreach (var game in Entities)
            {
                if (game_ids.Exists(id => id == game.Id))
                {
                    games.Add(game);
                }
            }

            return games;
        }
        
        public int Update(Game entity, bool saveChanges = true)
        {
            Game game = Entities.SingleOrDefault(e => e.Id == entity.Id);
            game.Title = entity.Title;
            game.Company = entity.Company;
            game.IGDB_Id = entity.IGDB_Id;
            game.Genres = entity.Genres;
            game.Summary = entity.Summary;

            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int UpdateGameListItem(GameListItem gameListItem, int idNewList, bool saveChanges = true)
        {

            GameListItem gameListItemDB = Context.GameListItems.Where(ml =>
                                        ml.Id_Games == gameListItem.Id_Games &&
                                        ml.Id_Users == gameListItem.Id_Users &&
                                        ml.Id_GameListCategories == gameListItem.Id_GameListCategories).SingleOrDefault();

            Context.GameListItems.Remove(gameListItemDB);
            
            Context.GameListItems.Add(new GameListItem
            {
                Id_Games = gameListItem.Id_Games,
                Id_GameListCategories = idNewList,
                Id_Users = gameListItem.Id_Users
            });

            if (saveChanges)
            {
                return Context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }
    }
}
