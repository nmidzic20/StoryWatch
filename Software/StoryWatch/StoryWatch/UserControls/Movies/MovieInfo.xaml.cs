using EntitiesLayer.Entities;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
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

namespace StoryWatch.UserControls.Movies
{
    /// <summary>
    /// Interaction logic for MovieInfo.xaml
    /// </summary>
    public partial class MovieInfo : UserControl
    {

        public Movie movie { get; set; }
        public MovieInfo(Movie movie)
        {
            InitializeComponent();

            this.movie = movie;
            this.DataContext = movie;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sHTML = "<!DOCTYPE html>" +
            "<html>" +
            "<head>" +
                "<meta charset=\"utf-8\" />" +
            "</head>" +
            "<body" +
                "<iframe width=\"560\" height=\"315\" src=\"https://www.youtube.com/embed/5PSNL1qE6VY\" title=\"YouTube video player\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share\" allowfullscreen></iframe>" +
            "</body>" +
            "</html>";

            string htmlBeginning = "<!DOCTYPE html>" +
                                    "<html>" +
                                    "<head>" +
                                        "<meta charset=\"utf-8\" />" +
                                        "<title>Test Title</title>" +
                                    "</head>" +
                                    "<body>";

            string trailer = "";
            
            if (!string.IsNullOrEmpty(movie.Trailer_URL))
                trailer = 
                    "<iframe width=\"560\" height=\"315\" src=\"https://www.youtube.com/embed/" 
                    + movie.Trailer_URL 
                    + "\" title=\"YouTube video player\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share\" allowfullscreen></iframe>";

            string htmlEnd = "</body>" +
                            "</html>";

            string html = htmlBeginning + trailer + htmlEnd;

            var env = await CoreWebView2Environment.CreateAsync();
            await webView2.EnsureCoreWebView2Async(env);
            webView2.CoreWebView2.NavigateToString(html);
        }

        private void WebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                ((WebView2)sender).ExecuteScriptAsync("document.querySelector('body').style.overflow='hidden'");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }
    }
}
