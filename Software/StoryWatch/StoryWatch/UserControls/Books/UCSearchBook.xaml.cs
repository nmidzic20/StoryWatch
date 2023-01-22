﻿using BusinessLayer;
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
using EntitiesLayer;

namespace StoryWatch.UserControls.Books
{
    /// <summary>
    /// Interaction logic for UCAddBook.xaml
    /// </summary>
    public partial class UCAddBook : UserControl
    {
        private BookService bookServices;
        private string title;
        public UCAddBook(string Title)
        {
            InitializeComponent();
            bookServices = new BookService();
            title = Title;
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
            List<Book> books = bookServices.returnCurrentBookList();
            Book currentBook = books.FirstOrDefault(b => b.Title.Contains(item));

            AddBook addBook = new AddBook(currentBook, title);
            addBook.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lbResults.Items.Clear();
            bookServices.clearList();
            if (string.IsNullOrEmpty(txtSearchKeyword.Text) || txtSearchKeyword.Text == "Search books by keyword")
                return;

            lbResults.Items.Clear();

            foreach(var book in bookServices.findBookByName(txtSearchKeyword.Text))
            {
                lbResults.Items.Add(book.Title);
            }
        }

        private void btnBooksHome_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCMediaHome(MediaCategory.Book));
        }

        private void btnAddBookManually_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook(title);
            addBook.Show();
        }
    }
}
