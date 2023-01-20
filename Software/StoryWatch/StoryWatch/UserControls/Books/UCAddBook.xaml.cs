using BusinessLayer;
using EntitiesLayer.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace StoryWatch.UserControls.Books
{
    /// <summary>
    /// Interaction logic for UCAddBook.xaml
    /// </summary>
    public partial class UCAddBook : UserControl
    {
        private ListCategoryServices listCategoryServices = new ListCategoryServices();
        private BookService bookServices = new BookService();

        List<Book> bookInfo;

        private string delimiter = " | ID: ";

        HttpClient bookClient = new HttpClient();
        public const string bookURL = "https://www.googleapis.com/books/v1/volumes/";
        public UCAddBook()
        {
            InitializeComponent();
            bookClient.BaseAddress = new Uri(bookURL);
            bookInfo = new List<Book>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtSearch = sender as TextBox;
            txtSearch.Text = "";
            txtSearch.FontStyle = FontStyles.Normal;
            txtSearch.FontWeight = FontWeights.Normal;
            txtSearch.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtSearch = sender as TextBox;

            if (txtSearch.Name == "txtSearchKeyword")
            
            txtSearch.FontStyle = FontStyles.Italic;
            txtSearch.FontWeight = FontWeights.Bold;
            txtSearch.Foreground = new SolidColorBrush(Colors.SlateGray);
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (lbResults == null ||
                string.IsNullOrEmpty(txtSearchKeyword.Text))
                return;
                

            lbResults.Items.Clear();
        }

        private void lbResults_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var item = lbResults.SelectedItem.ToString();   
            var a = bookInfo.FirstOrDefault(b => b.Title.Contains(item));

            MessageBox.Show("Title: " + a.Title + ", Autor: " + a.Author + ", Summary:" + a.Summary);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lbResults.Items.Clear();
            bookInfo.Clear();
            if (string.IsNullOrEmpty(txtSearchKeyword.Text))
                return;

            lbResults.Items.Clear();

            HttpResponseMessage response;
            string urlParameters = "?q=" + txtSearchKeyword.Text;
            response = bookClient.GetAsync(urlParameters).Result;

            if (response.IsSuccessStatusCode)
            {
                JObject bookJson = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                JArray books = (JArray)bookJson["items"];
                foreach (var book in books)
                {
                    JObject volumeInfoObject = (JObject)book["volumeInfo"];
                    JArray autor = (JArray)volumeInfoObject["authors"];
                    string title = (string)volumeInfoObject["title"];
                    string summary = (string)volumeInfoObject["description"];
                    if (autor != null)
                    {
                        string author = (string)autor[0];
                        Book bookAdd = new Book { Title = title, Summary = summary, Author = author };
                        bookInfo.Add(bookAdd);
                    }
                    else
                    {
                        Book bookAdd = new Book { Title = title, Summary = summary };
                        bookInfo.Add(bookAdd);
                    }
                    lbResults.Items.Add((string)volumeInfoObject["title"]);
                }
            }
        }
    }
}
