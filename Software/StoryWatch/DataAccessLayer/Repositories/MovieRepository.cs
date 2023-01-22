﻿using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class MovieRepository : Repository<Movie>
    {
        public int AddMovieToList(MovieListItem movieListItem, bool saveChanges = true)
        {
            Context.MovieListItems.Add(movieListItem);
            if (saveChanges)
            {
                return Context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int DeleteMovieFromList(MovieListItem movieListItem, bool saveChanges = true)
        {
            Context.MovieListItems.Attach(movieListItem);
            Context.MovieListItems.Remove(movieListItem);
            if (saveChanges)
            {
                return Context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public IQueryable<Movie> GetMovieByTitle(string title)
        {
            var query = from m in Entities
                        where m.Title == title
                        select m;

            return query;
        }

        public IQueryable<Movie> GetMovieByTMDBId(string TMDB_ID)
        {
            var query = from m in Entities
                        where m.TMDB_ID == TMDB_ID
                        select m;

            return query;
        }

        public IQueryable<Movie> GetMovieById(int id)
        {
            var query = from m in Entities
                        where m.Id == id
                        select m;

            return query;
        }

        public List<Movie> GetMoviesForList(MovieListCategory movieListCategory, User loggedUser)
        {
            //Context.Users.Attach(loggedUser);
            var user = Context.Users.Where(u => u.Id == loggedUser.Id).Single();
            var movie_ids = user.MovieListItems.Where(l => l.Id_MovieListCategories == movieListCategory.Id).Select(l => l.Id_Movies).ToList();

            List<Movie> movies = new List<Movie>();

            foreach (var movie in Entities)
                if (movie_ids.Exists(id => id == movie.Id))
                    movies.Add(movie);

            return movies;
        }

        public int Update(Movie entity, bool saveChanges = true)
        {
            var movie = Entities.SingleOrDefault(e => e.Id == entity.Id);
            movie.Title = entity.Title;
            movie.Description = entity.Description;
            movie.ReleaseDate = entity.ReleaseDate;
            movie.Countries = entity.Countries;

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
