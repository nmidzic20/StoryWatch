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

namespace StoryWatch
{
    /// <summary>
    /// Interaction logic for Registracija.xaml
    /// </summary>
    public partial class Registracija : Window
    {
        private readonly Window parent = null;

        public Registracija(Window parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            parent.Show();
        }
    }
}
