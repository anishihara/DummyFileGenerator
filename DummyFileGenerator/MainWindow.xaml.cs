using System;
using System.Collections.Generic;
using System.IO;
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

namespace DummyFileGenerator
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

        private void selectFolderButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                folderTextBox.Text = dialog.SelectedPath;
            }
        }

        private void createFileButtonClick(object sender, RoutedEventArgs e)
        {
            int filesize;
            try
            {
               filesize = Convert.ToInt32(fileSizeTextBox.Text);
            }
            catch (FormatException exp)
            {
                MessageBox.Show("Not valid filesize!");
                createFileButton.Content = "Generate dummy file!";
                return;
            }
            string filepath = string.Empty;
            if (System.IO.Directory.Exists(folderTextBox.Text))
            {
                filepath = System.IO.Path.Combine(folderTextBox.Text, filenameTextBox.Text);
            }
            else
            {
                if (MessageBox.Show("Directory not found! Create new directory?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.IO.Directory.CreateDirectory(folderTextBox.Text);
                    filepath = System.IO.Path.Combine(folderTextBox.Text, filenameTextBox.Text);
                }
                else
                {
                    MessageBox.Show("Cannot generate dummy file!");
                    return;
                }
            }
            createFileButton.IsEnabled = false;
            createFileButton.Content = "Creating...";
            try
            {
                using (var fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    byte[] b = new byte[1024];
                    b = b.Select(a => (byte)1).ToArray();
                    for (int i = 0; i < filesize * 1024; i++)
                    {
                        fs.Write(b, 0, 1024);
                    }
                    fs.Close();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                createFileButton.IsEnabled = true;
                createFileButton.Content = "Create file!";
            }
            
        }
    }
}
