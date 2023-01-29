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
                //MessageBox.Show(GuiManager.currentContent.GetType().Name);
                try
                {
                    System.Diagnostics.Process.Start(@"PDF\\" + GuiManager.currentContent.GetType().Name + ".pdf");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                //this.KeyDown -= UCHomee_KeyDown;
            }
        }
    }
}
