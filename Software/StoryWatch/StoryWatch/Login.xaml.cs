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
using System.Windows.Shapes;

namespace StoryWatch
{
    /// <summary>
    /// Interaction logic for LoginRegister.xaml
    /// </summary>
    public partial class Login : Window
    {
        private User user = new User
        {
            Id = 1,
            Username = "Korisnik1",
            Password = "Test"
        };

        private UserServices userServices;

        public Login()
        {
            InitializeComponent();
            userServices = new UserServices();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (!userServices.Login(new User()
            {
                Username = txtUsername.Text,
                Password = txtPassword.Password
            }))
            {
                MessageBox.Show("Netočno korisničko ime ili lozinka!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            var aplikacija = new MainWindow();
            aplikacija.Show();
            Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Registracija windowRegistracija = new Registracija(this);
            windowRegistracija.Show();
            Hide();
        }
    }
}
