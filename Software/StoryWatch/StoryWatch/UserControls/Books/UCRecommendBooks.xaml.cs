using BusinessLayer;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StoryWatch.UserControls.Books
{
    /// <summary>
    /// Interaction logic for UCRecommendBooks.xaml
    /// </summary>
    public partial class UCRecommendBooks : UserControl
    {

        private RecommendServices recommendServices;
        private BookService bookService;

        public UCRecommendBooks()
        {
            InitializeComponent();
            recommendServices = new RecommendServices(StateManager.LoggedUser);
            bookService = new BookService();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            List<Book> allDatabaseBooks = bookService.GetAll().ToList();
            PerformAnalysis(allDatabaseBooks);
        }

        private void PerformAnalysis(List<Book> allDatabaseBooks)
        {
            var mostRepeatedAuthor = allDatabaseBooks.GroupBy(x => x.Author)
                .OrderByDescending(x => x.Count())
                .Select(grp => grp.Key).First();

            var bookPagesLowerThan250 = allDatabaseBooks.Where(b => string.Compare(b.Pages,"250") <= 0).Count();
            var bookPagesGreaterThan250 = allDatabaseBooks.Where(b => string.Compare(b.Pages, "250") >= 0).Count();

            List<Book> booksByAuthor = bookService.findBooksByAuthor(mostRepeatedAuthor).ToList();
            
            if(bookPagesLowerThan250 > bookPagesGreaterThan250)
            {
                dgvRecommendedBooks.ItemsSource = booksByAuthor.Where(b => string.Compare(b.Pages, "250") <=0).ToList();
            }
            else
            {
                dgvRecommendedBooks.ItemsSource = booksByAuthor.Where(b => string.Compare(b.Pages,"250") >= 0).ToList();
            }
            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }

    }
}
