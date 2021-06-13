using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageP
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DatabaseManager databaseManager = DatabaseManager.getInstance();

        public MainWindow()
        {
            InitializeComponent();

            refreshList();
        }

        private void addPhoto(object sender, RoutedEventArgs e)
        {
            new AddREditImage().ShowDialog();
            refreshList();
        }

        public void refreshList()
        {
            var query = searchBox.Text;
            ImageList.ItemsSource = databaseManager.GetData(query);
        }

        private void searchData(object sender, TextChangedEventArgs e)
        {
            var query = (sender as TextBox).Text;
            ImageList.ItemsSource = databaseManager.GetData(query);
        }

        private void CopyImage(object sender, RoutedEventArgs e)
        {
            var path = ((sender as Button).DataContext as Image).Path;
            BitmapSource image = new BitmapImage(new Uri(path));
            Clipboard.SetImage(image);
        }

        private void onObjectClict(object sender, MouseButtonEventArgs e)
        {
            var dialog = new AddREditImage((sender as Grid).DataContext as Image);
            dialog.ShowDialog();
            refreshList();
        }
    }
}