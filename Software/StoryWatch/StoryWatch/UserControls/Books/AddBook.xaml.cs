using BusinessLayer;
using EntitiesLayer;
using EntitiesLayer.Entities;
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
        public string title
        public ListCategoryServices listCategoryServices;
        public AddBook(string Title)
        {
            InitializeComponent();
            bookService = new BookService();
            title = Title;
            listCategoryServices = new ListCategoryServices();
        }
        public AddBook(Book book, string Title)
        {
            InitializeComponent();
            currentBook = book;
            bookService = new BookService();
            title = Title;
            listCategoryServices = new ListCategoryServices(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTextBoxes();
        }

        private void LoadTextBoxes()
        {
            if(currentBook != null)
            {
                txtID.Text =  (currentBook.Id).ToString();
                txtTitle.Text = currentBook.Title;
                txtSummary.Text = currentBook.Summary;
                txtAuthor.Text = currentBook.Author;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            BookListCategory bc = listCategoryServices.CurrentBookListCategory(title);

            Book newBook = new Book {
                Id = int.Parse(txtID.Text),
                Title = txtTitle.Text,
                Summary = txtSummary.Text,
                Author = txtAuthor.Text,
                BookListCategories = bc
            };

            //Provjeriti da ne postoji knjiga sa istim ID-jem ili istim nazivom istim nazivom na istoj listi
            bookService.AddBook(newBook);
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
