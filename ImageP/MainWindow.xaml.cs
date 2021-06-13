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
using System.Windows.Shapes;

namespace ImageP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseManager databaseManager = DatabaseManager.getInstance();
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
            string query = searchBox.Text;
            ImageList.ItemsSource = databaseManager.GetData(query);
        }

        private void searchData(object sender, TextChangedEventArgs e)
        {
            string query = (sender as TextBox).Text;
            ImageList.ItemsSource = databaseManager.GetData(query);
        }

        private void CopyImage(object sender, RoutedEventArgs e)
        {
            string path = ((sender as Button).DataContext as Image).Path  ;
            BitmapSource image = new BitmapImage(new Uri(path) );
            Clipboard.SetImage(image);
        }

        private void onObjectClict(object sender, MouseButtonEventArgs e)
        {
            AddREditImage dialog = new AddREditImage((sender as Grid).DataContext as Image);
            dialog.ShowDialog();
            refreshList();
        }
    }
}
