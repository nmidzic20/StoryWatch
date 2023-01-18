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

namespace StoryWatch.UserControls
{
    /// <summary>
    /// Interaction logic for MediaListBox.xaml
    /// </summary>
    public partial class MediaListBox : UserControl
    {
        public MediaListBox(string title, string colorString)
        {
            InitializeComponent();

            lblTitle.Content = title;

            Color color = (Color)ColorConverter.ConvertFromString(colorString);
            lblTitle.Background = new SolidColorBrush(color);
        }
    }
}
