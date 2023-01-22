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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTextBoxes();
            txtID.IsReadOnly = true;
        }

        private void LoadTextBoxes()
        {
            var id = bookService.GetID().Count()+6;
            if (currentBook != null)
            {
                txtID.Text =  id.ToString();
                txtTitle.Text = currentBook.Title;
                txtSummary.Text = currentBook.Summary;
                txtAuthor.Text = currentBook.Author;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            var id = bookService.GetID().Count()+6;

            if (CheckInput())
            {
                Book newBook = new Book
                {
                    Id = id,
                    Title = txtTitle.Text,
                    Summary = txtSummary.Text,
                    Author = txtAuthor.Text,
                };

                bookService.AddBook(newBook);

                BookListItem newBookList = new BookListItem
                {
                    Id_BookListCategories = listCategory.Id,
                    Id_Books = id,
                    Id_Users = StateManager.LoggedUser.Id
                };

                bookService.AddBookToList(newBookList);

                Close();
            }
        }

        private bool CheckInput()
        {
            if(txtTitle.Text.Length > 100) 
            {
                MessageBox.Show("Book title lenght needs to be lower than 100!", "Title length!");
                return false;
            }
            else if(txtSummary.Text.Length > 200)
            {
                MessageBox.Show("Book summary lenght needs to be lower than 100!", "Book length!");
                return false;
            }
            else if(txtAuthor.Text.Length > 100)
            {
                MessageBox.Show("Book author lenght needs to be lower than 100!", "Author length!");
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
