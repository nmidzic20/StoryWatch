﻿using BusinessLayer;
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

        public UCRegistracija()
        {
            InitializeComponent();

        }

        private void Registracija_Click(object sender, RoutedEventArgs e)
        {
            // TODO: registracija radi, ali izbacuje grešku iz prvog if-a ???
            User user = new User()
            {
                Username = txtUsername.Text,
                Password = txtPassword1.Password,
            };

            if (userServices.Add(user) == 0)
            {
                MessageBox.Show("Greška u registraciji korisnika!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (userServices.Add(user) == -1)
            {
                MessageBox.Show("Korisničko ime je zauzeto!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Close();
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
    }
}

