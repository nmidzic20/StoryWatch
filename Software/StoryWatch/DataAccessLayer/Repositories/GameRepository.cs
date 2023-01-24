using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GameRepository : Repository<Game>
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
            var query = from m in Entities
                        where m.IGDB_Id == id
                        select m;

            return query;
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
                if (game_ids.Exists(id => id == game.Id))
                    games.Add(game);

            return games;
        }
        
        public int Update(Game entity, bool saveChanges = true)
        {
            var movie = Entities.SingleOrDefault(e => e.Id == entity.Id);
            movie.Title = entity.Title;
            movie.Company = entity.Company;
            movie.Indie = entity.Indie;

            if (saveChanges)
            {
                return SaveChanges();
            }
            else
            {
                return 0;
            }
        }
    }
}
