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

namespace StoryWatch.UserControls
{
    /// <summary>
    /// Interaction logic for UpdateCustomList.xaml
    /// </summary>
    public partial class UpdateCustomList : Window
    {
        private IListCategory listCategory;
        private ListCategoryServices listCategoryServices = new ListCategoryServices();

        public UpdateCustomList(IListCategory listCategory)
        {
            InitializeComponent();
            this.listCategory = listCategory;

            txtName.Text = listCategory.Title;
            clpck.SelectedColor = (Color)ColorConverter.ConvertFromString(listCategory.Color);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            listCategory.Title = txtName.Text;
            listCategory.Color = clpck.SelectedColor.ToString();
            listCategoryServices.UpdateListCategory(listCategory, StateManager.LoggedUser, StateManager.CurrentMediaCategory);
            GuiManager.OpenContent(new UCMediaHome(StateManager.CurrentMediaCategory));
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
