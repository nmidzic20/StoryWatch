using BusinessLayer;
using EntitiesLayer;
using EntitiesLayer.Entities;
using StoryWatch.UserControls.Books;
using StoryWatch.UserControls.Games;
using StoryWatch.UserControls.Movies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMDbLib.Objects.Movies;

namespace StoryWatch.UserControls
{
    /// <summary>
    /// Interaction logic for MediaListBox.xaml
    /// </summary>
    public partial class MediaListBox : UserControl
    {
        public IListCategory listCategory { get; set; }
        public ObservableCollection<Media> MediaItems = new ObservableCollection<Media>();

        public MediaListBox(IListCategory lc)
        {
            InitializeComponent();

            listCategory = lc;

            ModifyAppearance(lc);
            ShowMediaItems();
        }

        private void ShowMediaItems()
        {
            switch (StateManager.CurrentMediaCategory)
            {
                case MediaCategory.Movie:
                {
                    var movieServices = new MovieServices();
                    var movies = movieServices.GetMoviesForList(listCategory as MovieListCategory, StateManager.LoggedUser);

                    foreach (var movie in movies)
                    {
                        MediaItems.Add(movie);
                        //lbMedia.Items.Add(movie);
                    }

                    break;
                }
                case MediaCategory.Book:
                {
                    BookService bookServices = new BookService();
                    var books = bookServices.GetBooksForList(listCategory as BookListCategory, StateManager.LoggedUser);
                    foreach (var book in books)
                    {
                        MediaItems.Add(book);
                        //lbMedia.Items.Add(book.Title);
                    }
                    break;
                }
                case MediaCategory.Game:
                {
                    GameServices gameServices = new GameServices();
                    var games = gameServices.GetGamesForList(listCategory as GameListCategory, StateManager.LoggedUser);
                    foreach (var game in games)
                    {
                        MediaItems.Add(game);
                    }
                    break;
                }
            }

            lbMedia.DataContext = MediaItems;

        }

       

        private void ModifyAppearance(IListCategory lc)
        {
            lblTitle.Content = lc.Title;
            string colorString = lc.Color;

            if (colorString == "") colorString = "#FFFFFF";

            Color color = (Color)ColorConverter.ConvertFromString(colorString);
            header.Background = new SolidColorBrush(color);
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (StateManager.CurrentMediaCategory == MediaCategory.Movie)
            {
                GuiManager.OpenContent(new UCAddMovieToList(this.listCategory));
            }
            else if (StateManager.CurrentMediaCategory == MediaCategory.Book)
            {
                GuiManager.OpenContent(new UCAddBook(this.listCategory));
            }
            else
            {
                GuiManager.OpenContent(new UCAddGame(this.listCategory));
            }

        }

        private async void lbMedia_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (lbMedia.SelectedItem == null)
                return;

            if (StateManager.CurrentMediaCategory == MediaCategory.Movie)
            {
                var selectedMovie = lbMedia.SelectedItem as EntitiesLayer.Entities.Movie;
                var movieServices = new MovieServices();
                //var movie = await movieServices.GetMovieInfoAsync(int.Parse(selectedMovie.TMDB_ID));
                //MessageBox.Show(movie.Title + " " + movie.Tagline + " ");
            }
            else if(StateManager.CurrentMediaCategory == MediaCategory.Book)
            {
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (StateManager.CurrentMediaCategory == MediaCategory.Movie)
            {
                if (btn.DataContext is Media)
                {
                    EntitiesLayer.Entities.Movie movie = (EntitiesLayer.Entities.Movie) btn.DataContext;
                    GuiManager.OpenContent(new UCAddMovieToList(listCategory, movie));
                }
            }
            else if(StateManager.CurrentMediaCategory == MediaCategory.Book)
            {
                if(btn.DataContext is Media)
                {
                    Book selectedBook = btn.DataContext as Book;
                    AddBook addBook = new AddBook(selectedBook, listCategory, true);
                    addBook.Show();
                }
            }
            else
            {
                if (btn.DataContext is Media)
                {
                    Game selectedGame = btn.DataContext as Game;
                    AddGame addGame = new AddGame(listCategory, selectedGame);
                    addGame.Show();
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (StateManager.CurrentMediaCategory == MediaCategory.Movie)
            {
                if (btn.DataContext is Media)
                {
                    EntitiesLayer.Entities.Movie movie = (EntitiesLayer.Entities.Movie)btn.DataContext;
                    var movieServices = new MovieServices();
                    movieServices.DeleteMovieFromList(movie, listCategory as MovieListCategory, StateManager.LoggedUser);
                    GuiManager.OpenContent(new UCMediaHome(MediaCategory.Movie));
                }
            }
            else if(StateManager.CurrentMediaCategory == MediaCategory.Book)
            {
                if (btn.DataContext is Media)
                {
                    Book selectedBook = btn.DataContext as Book;
                    BookService bookServices = new BookService();
                    bookServices.DeleteBookFromList(selectedBook, listCategory as BookListCategory, StateManager.LoggedUser);
                    GuiManager.OpenContent(new UCMediaHome(MediaCategory.Book));
                }
            }
            else
            {
                if (btn.DataContext is Media)
                {
                    Game selectedGame = btn.DataContext as Game;
                    GameServices gameServices = new GameServices();
                    gameServices.DeleteGameFromList(selectedGame, listCategory as GameListCategory, StateManager.LoggedUser);
                    GuiManager.OpenContent(new UCMediaHome(MediaCategory.Game));
                }
            }
        }
    }
}
