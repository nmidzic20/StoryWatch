using BusinessLayer;
using EntitiesLayer;
using EntitiesLayer.Entities;
using IGDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// </summary>
    public partial class AddBook : Window
    {
        public Book currentBook;
        public BookService bookService;
        public IListCategory listCategory { get; set; }
        public bool update;
        public AddBook(IListCategory ListCategory)
        {
            InitializeComponent();
            bookService = new BookService();
            listCategory = ListCategory;
        }
        public AddBook(Book book, IListCategory ListCategory)
        {
            InitializeComponent();
            currentBook = book;
            bookService = new BookService();
            listCategory = ListCategory;
        }

        public AddBook(Book book, IListCategory ListCategory, bool Update)
        {
            InitializeComponent();
            update = Update;
            currentBook = book;
            bookService = new BookService();
            listCategory = ListCategory;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTextBoxes();
            txtID.IsReadOnly = true;
        }

        private void LoadTextBoxes()
        {
            var allBooks = bookService.GetAll().ToList();
            var id = (allBooks.Count() != 0) ? allBooks.Last().Id + 1 : 0;
            if (currentBook != null)
            {
                if (update)
                    txtID.Text = currentBook.Id.ToString();
                else
                    txtID.Text = id.ToString();
                txtTitle.Text = currentBook.Title;
                txtSummary.Text = currentBook.Summary;
                txtAuthor.Text = currentBook.Author;
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

            if (CheckInput())
            {
                Book newBook;
                if (currentBook != null)
                {
                    newBook = new Book
                    {
                        Id = id,
                        Title = txtTitle.Text,
                        Summary = txtSummary.Text,
                        Author = txtAuthor.Text,
                        PreviewURL = currentBook.PreviewURL
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
            Book updateBook = new Book
            {
                Id = currentBook.Id,
                Title = txtTitle.Text,
                Summary = txtSummary.Text,
                Author = txtAuthor.Text,
            };
            var update = bookService.UpdateBook(updateBook);

            if (update == 0)
                MessageBox.Show("Update failed! You didn't changed anything!");
            Close();
            GuiManager.OpenContent(new UCMediaHome(MediaCategory.Book));
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
