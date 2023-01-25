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
using System.Windows.Shapes;

namespace StoryWatch.UserControls.Books
{
    /// <summary>
    /// Interaction logic for E_book.xaml
    /// </summary>
    public partial class E_book : Window
    {
        string url;
        public E_book(string Url)
        {
            InitializeComponent();
            url = Url;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (url == null)
            {
                MessageBox.Show("Preview not found for this book", "Preview!");
                this.Close();
            }
            else
            {
                var env = await CoreWebView2Environment.CreateAsync();
                await webView2.EnsureCoreWebView2Async(env);
                webView2.CoreWebView2.Navigate(url);
            }
        }
    }
}
