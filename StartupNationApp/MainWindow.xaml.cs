using Microsoft.Win32;
using StartupNationApp.Utils;
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

namespace StartupNationApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChooseFileClick(object sender, RoutedEventArgs e)
        {
            InputFileTextBox.Text = ChooseFIleDialog.GetFilePath(checkFileExists: true);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to quit? there may be unsaved changes...", 
                "Quiting are we?", MessageBoxButton.YesNo);
            e.Cancel = result == MessageBoxResult.No;
        }
    }
}
