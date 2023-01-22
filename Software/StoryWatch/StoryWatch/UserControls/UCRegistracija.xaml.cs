using BusinessLayer;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StoryWatch.UserControls
{
    /// <summary>
    /// Interaction logic for UCRegistracija.xaml
    /// </summary>
    public partial class UCRegistracija : UserControl
    {
        private UserServices userServices;

        public UCRegistracija()
        {
            InitializeComponent();
            userServices = new UserServices();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            User user = new User()
            {
                Username = txtUsername.Text,
                Password = txtPassword1.Password,
            };


            int result = userServices.Add(user);

            if (result == 0)
            {
                MessageBox.Show("Greška u dodavanju korisnika!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (result == -1)
            {
                MessageBox.Show("Korisničko ime je zauzeto!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                MessageBox.Show("Korisnik registriran!", "Obavijest", MessageBoxButton.OK, MessageBoxImage.Information);

                StateManager.LoggedUser = user;

                var listCategorySerivces = new ListCategoryServices();
                listCategorySerivces.CreateDefaultLists(EntitiesLayer.MediaCategory.Movie, StateManager.LoggedUser);
                listCategorySerivces.CreateDefaultLists(EntitiesLayer.MediaCategory.Book, StateManager.LoggedUser);
                listCategorySerivces.CreateDefaultLists(EntitiesLayer.MediaCategory.Game, StateManager.LoggedUser);

                GuiManager.OpenContent(new UCLogin());
            }
        }

        private void InputChanged(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword1.Password) || string.IsNullOrWhiteSpace(txtPassword2.Password))
            {
                btnRegister.IsEnabled = false;
            }
            else if (txtPassword1.Password != txtPassword2.Password)
            {
                txtPassword1.Background = Brushes.Orange;
                btnRegister.IsEnabled = false;
            }
            else
            {
                txtPassword1.Background = Brushes.White;
                btnRegister.IsEnabled = true;
            }
        }

        private void ButtonBackClicked(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UCLogin());
        }
    }
}

