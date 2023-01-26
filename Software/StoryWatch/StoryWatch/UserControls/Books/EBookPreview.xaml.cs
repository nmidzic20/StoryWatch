﻿using EntitiesLayer.Entities;
using Microsoft.Web.WebView2.Core;
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
    /// Interaction logic for EBookPreview.xaml
    /// </summary>
    public partial class EBookPreview : UserControl
    {
        Book book;
        public EBookPreview(Book Book)
        {
            InitializeComponent();
            book = Book;
            PrepareFormWithData();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (book.PreviewURL == null)
            {
                MessageBox.Show("Preview not found for this book", "Preview!");
            }
            else
            {
                var env = await CoreWebView2Environment.CreateAsync();
                await webView2.EnsureCoreWebView2Async(env);
                webView2.CoreWebView2.Navigate(book.PreviewURL);
            }
        }

        private void PrepareFormWithData()
        {
            stackPanelInfo.Visibility = Visibility.Collapsed;
            txtTitle.IsReadOnly = true;
            txtSummary.IsReadOnly = true;
            txtAuthor.IsReadOnly = true;
            txtPages.IsReadOnly = true;
            txtTitle.Text = book.Title;
            txtAuthor.Text = book.Author;
            txtSummary.Text = book.Summary;
            txtPages.Text = book.Pages;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }

       

        private void btnHideInfo_Click(object sender, RoutedEventArgs e)
        { 
            stackPanelInfo.Visibility = Visibility.Collapsed;
            btnShowInfoFromDatabase.Visibility = Visibility.Visible;
        }

        private void btnShowInfoFromDatabase_Click(object sender, RoutedEventArgs e)
        {
            stackPanelInfo.Visibility = Visibility.Visible;
            btnShowInfoFromDatabase.Visibility = Visibility.Hidden;
        }
    }
}
