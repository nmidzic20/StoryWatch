using EntitiesLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IGDB;
using DataAccessLayer.Repositories;
using IGDB.Models;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;
using TMDbLib.Objects.Movies;

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
            return await api.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query: $"fields name, id, summary, version_parent; search \"{name}\"; where version_parent = null;");
        }

        public async Task<IGDB.Models.Game[]> GetHighestRatedGames()
        {
            return await api.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query: $"fields name, summary, aggregated_rating; sort aggregated_rating desc; limit 100;");
        }

        public async Task<IGDB.Models.Game[]> GetRecommendedGamesAsync(int[] ids)
        {
            List<IGDB.Models.Game> games = new List<IGDB.Models.Game>();
            
            for (int i = 0; i < ids.Length; i++)
            {
                games.AddRange(await api.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query: $"fields name, summary, version_parent; where id = {ids[i]} & version_parent = null; limit 5;"));
            }

            return games.ToArray();
        }

        public async Task<IGDB.Models.Genre[]> GetGameGenresAsync(int id)
        {
            return await api.QueryAsync<IGDB.Models.Genre>(IGDBClient.Endpoints.Genres, query: $"fields name; where id = {id};");
        }

        public List<EntitiesLayer.Entities.Game> GetAllGames()
        {
            using (var repo = new GameRepository())
            {
                return repo.GetAll().ToList();
            }
        }

        public EntitiesLayer.Entities.Game GetGameByTitle(string title)
        {
            using (var repo = new GameRepository())
            {
                return repo.GetGameByTitle(title).FirstOrDefault();
            }
        }

        public List<EntitiesLayer.Entities.Game> GetGamesForList(GameListCategory gameListCategory, User loggedUser)
        {
            using (var repo = new GameRepository())
            {
                return repo.GetGamesForList(gameListCategory, loggedUser);
            }
        }

        public EntitiesLayer.Entities.Game GetGameByIGDBId(string IGDB_ID)
        {
            using (var repo = new GameRepository())
            {
                return repo.GetGameByIGDBId(IGDB_ID).FirstOrDefault();
            }
        }

        public bool AddGame(EntitiesLayer.Entities.Game game)
        {
            bool isSuccessful = false;
            
            using (var repo = new GameRepository())
            {
                EntitiesLayer.Entities.Game existingGame = null;

                if (!string.IsNullOrEmpty(game.IGDB_Id))
                {
                    existingGame = repo.GetGameByIGDBId(game.IGDB_Id).FirstOrDefault();
                }

                if (existingGame == null)
                {
                    isSuccessful = true;
                    int affectedRows = repo.Add(game);
                    isSuccessful = affectedRows > 0;
                }
            }
            return isSuccessful;
        }
        
        public int UpdateGame(EntitiesLayer.Entities.Game game)
        {
            using (var repo = new GameRepository())
            {
                return repo.Update(game);
            }
        }

        public bool AddGameToList(GameListItem gameListItem, GameListCategory gameListCategory, User loggedUser)
        {
            bool isSuccessful = false;
            using (var repo = new GameRepository())
            {
                List<EntitiesLayer.Entities.Game> games = repo.GetGamesForList(gameListCategory, loggedUser).ToList();
                bool gameExistsInList = games.Exists(g => g.Id == gameListItem.Id_Games);

                if (gameExistsInList)
                {
                    isSuccessful = false;
                }
                else
                {
                    int affectedRows = repo.AddGameToList(gameListItem);
                    isSuccessful = affectedRows > 0;
                }
            }
            return isSuccessful;
        }

        public bool UpdateGameToAnotherList(GameListItem gameListItem, GameListCategory destGameListCategory, User loggedUser)
        {
            bool isSuccessful = false;
            using (var repo = new GameRepository())
            {
                //check if exists on destination list already, if yes, return false, if no, change to that list
                List<EntitiesLayer.Entities.Game> games = repo.GetGamesForList(destGameListCategory, loggedUser).ToList();
                bool gameExistsInList = games.Exists(m => m.Id == gameListItem.Id_Games);

                if (gameExistsInList)
                {
                    isSuccessful = false;
                }
                else
                {
                    //fetch movieListItem for this movie, this list and this user
                    //change the list
                    int affectedRows = repo.UpdateGameListItem(gameListItem, destGameListCategory.Id);
                    isSuccessful = affectedRows > 0;
                }

            }

            return isSuccessful;
        }

        public bool DeleteGameFromList(EntitiesLayer.Entities.Game game, GameListCategory gameListCategory, User loggedUser)
        {
            var gameListItem = new GameListItem
            {
                Id_GameListCategories = gameListCategory.Id,
                Id_Games = game.Id,
                Id_Users = loggedUser.Id
            };

            /*delete genre if it remains unused by any media - implement this when it can be tested
            using (var db = new GenreRepository())
            {
                var gameGenre = GetGameByTitle(game.Title).Genre;
                db.DeleteGenreWithoutMedia(gameGenre);
            }*/

            using (var repo = new GameRepository())
            {
                int affectedRows = repo.DeleteGameFromList(gameListItem);
                bool isSuccessful = affectedRows > 0;
                return isSuccessful;
            }
        }

        public async Task<IGDB.Models.Game> GetGameInfoAsync(int gameIGDBId)
        {
            return (await api.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games, query: $"fields name, genres.name, id, involved_companies.company.name, first_release_date, summary, similar_games; where id = {gameIGDBId};")).First();
        }
    }
}
