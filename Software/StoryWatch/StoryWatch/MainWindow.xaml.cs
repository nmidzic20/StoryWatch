using StoryWatch.UserControls;
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

using BusinessLayer;
using StoryWatch.UserControls.Movies;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StoryWatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GuiManager.MainWindow = this;
            GuiManager.OpenContent(new UCLogin());

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                try
                {
                    if (GuiManager.currentContent.GetType().Name == "UCAddMovieToList")
                    {
                        CheckUCAddMovie();
                    }
                    else if (GuiManager.currentContent.GetType().Name == "UCMediaHome")
                    {
                        CheckUCMediaHome();
                    }
                    else //any UC regardless of media category
                        System.Diagnostics.Process.Start(@"PDF\\" + GuiManager.currentContent.GetType().Name + ".pdf");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " - Ime PDF-a neka bude isto kao ime ove klase User Controle na kojoj se otvara, ime ove UC klase je " + GuiManager.currentContent.GetType().Name);
                }
            }
        }

        //user manuals for UCMediaHome by specific Media category
        private void CheckUCMediaHome()
        {
            UCMediaHome uc = GuiManager.currentContent as UCMediaHome;

            if (StateManager.CurrentMediaCategory == EntitiesLayer.MediaCategory.Movie)
                System.Diagnostics.Process.Start(@"PDF\\" + GuiManager.currentContent.GetType().Name + "Movies.pdf");
            else if (StateManager.CurrentMediaCategory == EntitiesLayer.MediaCategory.Book)
                System.Diagnostics.Process.Start(@"PDF\\" + GuiManager.currentContent.GetType().Name + "Books.pdf");
            else if (StateManager.CurrentMediaCategory == EntitiesLayer.MediaCategory.Game)
                System.Diagnostics.Process.Start(@"PDF\\" + GuiManager.currentContent.GetType().Name + "Games.pdf");

        }

        //specific user manual for movie UC which has multiple manuals associated with one user control
        private static void CheckUCAddMovie()
        {
            UCAddMovieToList uc = GuiManager.currentContent as UCAddMovieToList;

            if (uc.btnAdd.Content.ToString() == "Update") //update form
                System.Diagnostics.Process.Start(@"PDF\\" + GuiManager.currentContent.GetType().Name + "Update.pdf");
            else if (!string.IsNullOrEmpty(uc.txtID.Text)) //movie info pulled from TMDB
                System.Diagnostics.Process.Start(@"PDF\\" + GuiManager.currentContent.GetType().Name + "TMDB.pdf");
            else //manual movie info
                System.Diagnostics.Process.Start(@"PDF\\" + GuiManager.currentContent.GetType().Name + ".pdf");
        }
    }
}
