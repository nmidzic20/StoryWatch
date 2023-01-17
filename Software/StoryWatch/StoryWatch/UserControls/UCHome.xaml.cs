﻿using System;
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
    /// Interaction logic for UCHome.xaml
    /// </summary>
    public partial class UCHome : UserControl
    {
        public UCHome()
        {
            InitializeComponent();
        }

        private void btnBooks_Click(object sender, RoutedEventArgs e)
        {
           GuiManager.OpenContent(new UCMediaHome());
        }

        private void btnMovies_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCMediaHome());
        }

        private void btnGames_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCMediaHome());
        }
    }
}
