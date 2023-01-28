using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StoryWatch
{
    internal class GuiManager
    {
        public static MainWindow MainWindow { get; set; }

        private static UserControl currentContent;
        public static UserControl previousContent;

        public static void OpenContent(UserControl userControl)
        {
            previousContent = MainWindow.contentPanel.Content as UserControl;
            MainWindow.contentPanel.Content = userControl;
            currentContent = MainWindow.contentPanel.Content as UserControl;
        }

        public static void CloseContent()
        {
            OpenContent(previousContent);
        }
    }
}
