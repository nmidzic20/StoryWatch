using BusinessLayer;
using EntitiesLayer.Entities;
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
    /// Interaction logic for UCLogin.xaml
    /// </summary>
    public partial class UCLogin : UserControl
    {
        private UserServices userServices;

        public UCLogin()
        {
            InitializeComponent();
            userServices = new UserServices();

        }
        private User user = new User
        {
            Id = 1,
            Username = "Korisnik1",
            Password = "Test"
        };


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Ispunite podatke!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!userServices.Login(new User()
            {
                Username = txtUsername.Text,
                Password = txtPassword.Password
            }))
            {
                MessageBox.Show("Netočno korisničko ime ili lozinka!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            GuiManager.OpenContent(new UCHome());
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCRegistracija());
        }
    }
}
