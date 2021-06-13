using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace ImageP
{
    /// <summary>
    ///     Interaction logic for AddREditImage.xaml
    /// </summary>
    public partial class AddREditImage : Window
    {
        private readonly DatabaseManager databaseManager = DatabaseManager.getInstance();
        public bool editing;
        private readonly Image image;

        public AddREditImage()
        {
            image = new Image();
            DataContext = image;
            InitializeComponent();
            lwTags.ItemsSource = image.Tags;
        }

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
            var openFileDialog = new OpenFileDialog();
            if ((bool) openFileDialog.ShowDialog()) image.Path = openFileDialog.FileName;
        }

        private void addTag(object sender, RoutedEventArgs e)
        {
            image.Tags.Add(TagTB.Text);
            TagTB.Text = "";
        }

        private void saveImage(object sender, RoutedEventArgs e)
        {
            if (editing)
                databaseManager.UpdateData(image);
            else
                databaseManager.AddData(image);
            Close();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void deleteTag(object sender, RoutedEventArgs e)
        {
            var tag = (sender as Button).DataContext as string;
            image.Tags.Remove(tag);
        }

        private void TagTB_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) addTag(null, null);
        }
    }
}