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
using System.Windows.Shell;
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

        private ListCategoryServices listCategoryServices = new ListCategoryServices();

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

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (StateManager.CurrentMediaCategory == MediaCategory.Movie)
            {
                if (btn.DataContext is Media)
                {
                    EntitiesLayer.Entities.Movie movie = (EntitiesLayer.Entities.Movie)btn.DataContext;
                    GuiManager.OpenContent(new UCAddMovieToList(listCategory, movie));
                }
            }
            else if (StateManager.CurrentMediaCategory == MediaCategory.Book)
            {
                if (btn.DataContext is Media)
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

                    AddGame addGame = new AddGame(listCategory, selectedGame, true);
                    addGame.Show();
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return;
            }

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
            else if (StateManager.CurrentMediaCategory == MediaCategory.Book)
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
                    Game selectedBook = btn.DataContext as Game;
                    GameServices gameServices = new GameServices();
                    gameServices.DeleteGameFromList(selectedBook, listCategory as GameListCategory, StateManager.LoggedUser);
                    GuiManager.OpenContent(new UCMediaHome(MediaCategory.Game));
                }
            }
        }

        class DragDropData
        {
            public Media MediaItem { get; set; }
            public IListCategory SourceList { get; set; }
            public MediaListBox UCMediaListBox { get; set; }
        }

        Point startPoint;
        private void List_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Store the mouse position
            startPoint = e.GetPosition(null);
        }

        private void List_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListBox listBox = sender as ListBox;
                ListBoxItem listBoxItem = FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);
                if (listBoxItem == null) return;

                // Find the data behind the ListViewItem
                Media mediaItem = listBox.ItemContainerGenerator.ItemFromContainer(listBoxItem) as Media;

                // Initialize the drag & drop operation
                var dragDropData = new DragDropData
                {
                    MediaItem = mediaItem,
                    SourceList = listCategory,
                    UCMediaListBox = this
                };
                DataObject dragData = new DataObject("sourceListMediaItemInfo", dragDropData);
                DragDrop.DoDragDrop(listBoxItem, dragData, DragDropEffects.Move);

                //remove that media from list when the drop finalizes (here is a wait from the above line until drop is complete)
                MediaItems.Remove(mediaItem);
                listBox.Items.Refresh();

            }
        }

        // Helper to search up the VisualTree
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void DropList_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("sourceListMediaItemInfo") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void DropList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("sourceListMediaItemInfo"))
            {
                var data = e.Data.GetData("sourceListMediaItemInfo") as DragDropData;
                Media mediaItem = data.MediaItem;
                IListCategory sourceList = data.SourceList;

                ListBox listBox = sender as ListBox;
                MediaItems.Add(mediaItem);
                listBox.Items.Refresh();

                //update that media in database, from that list to this list
                //in case that media already on this list, need to visually undo drop

                var ucParent = FindAncestor<UserControl>(listBox);
                var userControl = ucParent as MediaListBox;
                IListCategory destinationList = userControl.listCategory;

                bool isSuccessful = false;

                switch (StateManager.CurrentMediaCategory)
                {
                    case MediaCategory.Movie:
                        isSuccessful = UpdateListMovie(mediaItem, sourceList, destinationList);
                        break;

                    case MediaCategory.Game:
                        isSuccessful = UpdateListGame(mediaItem, sourceList, destinationList);
                        break;

                    case MediaCategory.Book:
                        isSuccessful = UpdateBookList(mediaItem, sourceList, destinationList);
                        break;
                }

                if (!isSuccessful)
                {
                    MediaItems.Remove(mediaItem);
                    listBox.Items.Refresh();

                    var mbox = new Xceed.Wpf.Toolkit.MessageBox
                    {
                        Text = "This title has already been added to the destination list!"
                    };
                    mbox.ShowDialog();

                    data.UCMediaListBox.MediaItems.Add(mediaItem);
                    data.UCMediaListBox.lbMedia.Items.Refresh();
                }

            }
        }

        private static bool UpdateListMovie(Media mediaItem, IListCategory sourceList, IListCategory destinationList)
        {
            var movieServices = new MovieServices();
            var movieListItem = new MovieListItem
            {
                Id_MovieListCategories = sourceList.Id,
                Id_Movies = mediaItem.Id,
                Id_Users = StateManager.LoggedUser.Id
            };

            var isSuccessful = movieServices.UpdateMovieToAnotherList(movieListItem, destinationList as MovieListCategory, StateManager.LoggedUser);
            return isSuccessful;
        }

        private static bool UpdateBookList(Media mediaItem, IListCategory sourceList, IListCategory destinationList)
        {
            BookService bookService = new BookService();
            BookListItem bookListItem = new BookListItem
            {
                Id_BookListCategories = sourceList.Id,
                Id_Books = mediaItem.Id,
                Id_Users = StateManager.LoggedUser.Id
            };
            var isSuccessful = bookService.UpdateBookToAnotherList(bookListItem, destinationList as BookListCategory, StateManager.LoggedUser);
            return isSuccessful;

        }

        private static bool UpdateListGame(Media mediaItem, IListCategory sourceList, IListCategory destinationList)
        {
            var gameServices = new GameServices();
            var gameListItem = new GameListItem
            {
                Id_GameListCategories = sourceList.Id,
                Id_Games = mediaItem.Id,
                Id_Users = StateManager.LoggedUser.Id
            };

            var isSuccessful = gameServices.UpdateGameToAnotherList(gameListItem, destinationList as GameListCategory, StateManager.LoggedUser);

            return isSuccessful;
        }


        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            if (lbMedia.SelectedItem == null)
                return;

            if (StateManager.CurrentMediaCategory == MediaCategory.Movie)
            {
                Button button = sender as Button;
                EntitiesLayer.Entities.Movie movie = button.DataContext as EntitiesLayer.Entities.Movie;

                if (movie != null)
                {
                    //MessageBox.Show(movie.Trailer_URL);
                    GuiManager.OpenContent(new MovieInfo(movie));

                }

            }

            if(StateManager.CurrentMediaCategory == MediaCategory.Book)
            {
                Button button = sender as Button;
                Book book = button.DataContext as Book;

                if (book != null)
                {
                    if (book.PreviewURL != null)
                        GuiManager.OpenContent(new EBookPreview(book));
                    else
                        MessageBox.Show("This book does not support e-book preview", "E-book preview!");
                }
            }
        }

        private void btnUpdateList_Click(object sender, RoutedEventArgs e)
        {
            var updateListWindow = new UpdateCustomList(listCategory);
            updateListWindow.ShowDialog();
        }

        private void btnDeleteList_Click(object sender, RoutedEventArgs e)
        {
            listCategoryServices.DeleteListCategory(listCategory, StateManager.LoggedUser, StateManager.CurrentMediaCategory);
            GuiManager.OpenContent(new UCMediaHome(StateManager.CurrentMediaCategory));
        }
    }
}
