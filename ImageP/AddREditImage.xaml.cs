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
using Microsoft.Win32;

namespace ImageP
{
    /// <summary>
    /// Interaction logic for AddREditImage.xaml
    /// </summary>
    public partial class AddREditImage : Window
    {
        private Image image;
        public Boolean editing= false;
        public AddREditImage()
        {
            image = new Image();
            DataContext = image;
            InitializeComponent();
            lwTags.ItemsSource = image.Tags;
        }
        //TODO TAG SISTEMI KAYDETME 
        public AddREditImage(Image imag)
        {
            image = imag;
            DataContext = image;
            
            InitializeComponent();
            lwTags.ItemsSource = image.Tags;
            editing = true;
        }

        private void selectImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog  = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
            {
                image.Path=openFileDialog.FileName;
            }
        }

        private void addTag(object sender, RoutedEventArgs e)
        {
            image.Tags.Add(TagTB.Text);
            TagTB.Text = "";
        }
        DatabaseManager databaseManager=DatabaseManager.getInstance();
        private void saveImage(object sender, RoutedEventArgs e)
        {
            if (editing)
            {
                databaseManager.UpdateData(image);
            }
            else
            {
                databaseManager.AddData(image);

            }
            Close();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void deleteTag(object sender, RoutedEventArgs e)
        {
            string tag =(sender as Button).DataContext as string;
            image.Tags.Remove(tag);
            
        }

        private void TagTB_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                addTag(null, null);
            }
        }
    }
}
