using BusinessLayer;
using EntitiesLayer;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using TMDbLib.Objects.Movies;

namespace StoryWatch.UserControls.Books
{
    /// <summary>
    /// Interaction logic for AddBook.xaml
    /// Author: David Kajzogaj
    /// </summary>
    public partial class AddBook : Window
    {
        public Book currentBook;
        public BookService bookService;
        public GenreServices genreServices;
        public IListCategory listCategory { get; set; }
        public bool update;
        public AddBook(IListCategory ListCategory)
        {
            InitializeComponent();
            bookService = new BookService();
            genreServices = new GenreServices();
            listCategory = ListCategory;
        }
        public AddBook(Book book, IListCategory ListCategory)
        {
            InitializeComponent();
            currentBook = book;
            bookService = new BookService();
            genreServices = new GenreServices();
            listCategory = ListCategory;
        }

        public AddBook(Book book, IListCategory ListCategory, bool Update)
        {
            InitializeComponent();
            update = Update;
            currentBook = book;
            bookService = new BookService();
            genreServices = new GenreServices();
            listCategory = ListCategory;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTextBoxes();
        }

        private void LoadTextBoxes()
        {
            txtID.IsReadOnly = true;
            var allBooks = bookService.GetAll().ToList();
            var id = (allBooks.Count() != 0) ? allBooks.Last().Id + 1 : 0;
            if (currentBook != null)
            {
                if (update)
                {
                    var genre = bookService.GetBookById(currentBook.Id).Genre;
                    txtID.Text = currentBook.Id.ToString();
                    txtGenre.Text = genre.Name;
                }
                else
                    txtID.Text = id.ToString();
                txtTitle.Text = currentBook.Title;
                txtSummary.Text = currentBook.Summary;
                txtAuthor.Text = currentBook.Author;
                txtPages.Text = currentBook.Pages;
            }
            txtID.Text = id.ToString();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (update) 
            {
                UpdateBook();
            }
            else
            {
                AddNewBook();
            }
        }

        private void AddNewBook()
        {
            var allBooks = bookService.GetAll().ToList();

            var id = (allBooks.Count() != 0) ? allBooks.Last().Id + 1 : 0;

            if (CheckInput() && CheckEmptyInput())
            {
                Genre genre;
                int genreId = (genreServices.GetAllGenres().LastOrDefault() != null) ? genreServices.GetAllGenres().Last().Id + 1 : 0;
                genre = new Genre
                {
                    Id = genreId,
                    Name = txtGenre.Text
                };
                var dohvaceniGenre = genreServices.AddGenre(genre).Id;
                genre = genreServices.GetGenreById(dohvaceniGenre);

                Book newBook;
                if (currentBook != null)
                {
                    newBook = new Book
                    {
                        Id = id,
                        Title = txtTitle.Text,
                        Summary = txtSummary.Text,
                        Author = txtAuthor.Text,
                        PreviewURL = currentBook.PreviewURL,
                        Pages = currentBook.Pages,
                        Genre = genre,
                    };
                  
                }
                else
                {
                    newBook = new Book
                    {
                        Id = id,
                        Title = txtTitle.Text,
                        Summary = txtSummary.Text,
                        Author = txtAuthor.Text,
                        Pages = txtPages.Text,
                        Genre = genre,
                    };
                }

                Book addBook = bookService.AddBook(newBook);
                var foundBookId = id;
                if (addBook != null)
                    foundBookId = addBook.Id;

                BookListItem newBookList = new BookListItem
                {
                    Id_BookListCategories = listCategory.Id,
                    Id_Books = foundBookId,
                    Id_Users = StateManager.LoggedUser.Id
                };

                bool addBookList = bookService.AddBookToList(newBook ,newBookList, listCategory as BookListCategory, StateManager.LoggedUser);

                if (!addBookList)
                {
                    MessageBox.Show("This book is already added to this list!");
                }

                Close();
                GuiManager.OpenContent(new UCMediaHome(MediaCategory.Book));
            }
        }

        private void UpdateBook()
        {
            if (CheckInput() && CheckEmptyInput())
            {
                var oldGenre = bookService.GetBookById(currentBook.Id).Genre;
                var genre = UpdateGenre(oldGenre);

                Book updateBook = new Book
                {
                    Id = currentBook.Id,
                    Title = txtTitle.Text,
                    Summary = txtSummary.Text,
                    Author = txtAuthor.Text,
                    Pages = txtPages.Text,
                    Genre = genre
                };
                var update = bookService.UpdateBook(updateBook);

                if (update == 0)
                    MessageBox.Show("Update failed! You didn't change anything!");
                Close();
                GuiManager.OpenContent(new UCMediaHome(MediaCategory.Book));
            }
        }

        private Genre UpdateGenre(Genre oldGenre)
        {
            int genreId = (genreServices.GetAllGenres().LastOrDefault() != null) ? genreServices.GetAllGenres().Last().Id + 1 : 0;
            var newGenre = new Genre
            {
                Id = genreId,
                Name = txtGenre.Text
            };
            return genreServices.UpdateGenre(oldGenre, newGenre);
        }

        private bool CheckInput()
        {
            if(txtTitle.Text.Length > 100) 
            {
                MessageBox.Show("Book title length needs to be lower than 100!", "Title length!");
                return false;
            }
            else if(txtSummary.Text.Length > 900)
            {
                MessageBox.Show("Book summary length needs to be lower than 900!", "Book length!");
                return false;
            }
            else if(txtAuthor.Text.Length > 100)
            {
                MessageBox.Show("Book author length needs to be lower than 100!", "Author length!");
                return false;
            }
            return true;
        }
        private bool CheckEmptyInput()
        {
            int parsedValue;
            if (txtTitle.Text == "")
            {
                MessageBox.Show("Book title TextBox Empty!", "Title TextBox Empty!");
                return false;
            }
            else if (txtSummary.Text == "")
            {
                MessageBox.Show("Book summary TextBox Empty!", "Book TextBox Empty!");
                return false;
            }
            else if (txtAuthor.Text == "")
            {
                MessageBox.Show("Book author TextBox Empty!", "Author TextBox is Empty!");
                return false;
            }
            else if (txtPages.Text == "")
            {
                MessageBox.Show("Book pages TextBox Empty!", "Pages TextBox is Empty!");
                return false;
            }
            else if (txtGenre.Text == "")
            {
                MessageBox.Show("Book genre TextBox Empty!", "Genre TextBox is Empty!");
                return false;
            }
            else if (!int.TryParse(txtPages.Text, out parsedValue))
            {
                MessageBox.Show("Book pages TextBox is a number field only!", "Pages TextBox is number field!");
                return false;
            }
            return true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddBookk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1 && (GuiManager.currentContent.GetType().Name == "UCAddBook") && !update && (currentBook == null))
            {
                System.Diagnostics.Process.Start(@"PDF\\AddBookManually.pdf");

            }
            if (e.Key == Key.F1 && (GuiManager.currentContent.GetType().Name == "UCMediaHome") && (update != false)) //Ako se pokrene preko Update forme
            {
                System.Diagnostics.Process.Start(@"PDF\\UpdateBook.pdf");

            }
            if (e.Key == Key.F1 && (GuiManager.currentContent.GetType().Name == "UCAddBook") && (currentBook != null))
            {
                System.Diagnostics.Process.Start(@"PDF\\SelectedBook.pdf");
            }
        }
    }
}
