using BusinessLayer;
using EntitiesLayer.Entities;
using Microsoft.Reporting.WinForms;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StoryWatch.UserControls.Books
{
    /// <summary>
    /// Interaction logic for BookReport.xaml
    /// Author: David Kajzogaj
    /// </summary>
    public partial class BookReport : Window
    {
        private bool _isReportViewerLoaded;
        List<Book> allBooks;
        string favouriteBookAuthor;
        List<Book> pageDistribution;
        List<Book> favouriteBooks;
        List<Book> favoriteAuthor;
        BookService bookService;
        ListCategoryServices listCategoryServices;
        Window window;
        public BookReport()
        {
            InitializeComponent();
            this.allBooks = new List<Book>();
            this.pageDistribution = new List<Book>();
            favoriteAuthor = new List<Book>();
            this.favouriteBooks = new List<Book>();
            bookService = new BookService();
            listCategoryServices = new ListCategoryServices();

            reportViewer.Load += Window_Loaded;
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {

                GenerateReport();

                _isReportViewerLoaded = true;
            }

            window = Window.GetWindow(this);
            window.KeyDown += new KeyEventHandler(BookReportt_KeyDown);
        }

        private void GenerateReport()
        {
            reportViewer.LocalReport.DataSources.Clear();

            var userLists = listCategoryServices.GetBookListCategoriesForUser(StateManager.LoggedUser);

            foreach (var list in userLists)
            {
                if (list.Title == "Favorites")
                    favouriteBooks = bookService.GetBooksForList(list, StateManager.LoggedUser);
            }

            allBooks = bookService.GetAll().ToList();

            favouriteBookAuthor = allBooks.GroupBy(x => x.Author)
                .OrderByDescending(x => x.Count())
                .Select(grp => grp.Key).First();

            Book favAuthor = new Book
            {
                Author = favouriteBookAuthor,
            };
            favoriteAuthor.Add(favAuthor);

            var bookPagesLowerThan250 = allBooks.Where(b => string.Compare(b.Pages, "250") <= 0).Count();
            var bookPagesGreaterThan250 = allBooks.Where(b => string.Compare(b.Pages, "250") >= 0).Count();

            Book pagesLow250 = new Book
            {
                Pages = bookPagesLowerThan250.ToString()
            };

            pageDistribution.Add(pagesLow250);

            Book pagesGreat250 = new Book
            {
                Pages = bookPagesGreaterThan250.ToString()
            };

            pageDistribution.Add(pagesGreat250);

            var dataSource = new ReportDataSource() { Name = "Book", Value = favouriteBooks };
            reportViewer.LocalReport.DataSources.Add(dataSource);

            var dataSource2 = new ReportDataSource() { Name = "FavouriteBook", Value = favoriteAuthor };
            reportViewer.LocalReport.DataSources.Add(dataSource2);

            var dataSource3 = new ReportDataSource() { Name = "BookPagesDistribution", Value = pageDistribution };
            reportViewer.LocalReport.DataSources.Add(dataSource3);

            string path = "UserControls/Books/Reports/ReportBooks.rdlc"; 
            reportViewer.LocalReport.ReportPath = path;
            reportViewer.Refresh();
            reportViewer.RefreshReport();

        }

        private void BookReportt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F1 && (GuiManager.currentContent.Name == "BookReportt"))
            {
                System.Diagnostics.Process.Start(@"PDF\\BookReport");
                window.KeyDown -= BookReportt_KeyDown;
            }
        }
    }
}
