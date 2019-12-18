using System.Windows;
using EVL.Controllers;
using EVL.Model;

namespace EVL
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainController mainController;

        public MainWindow(MainController mainController)
        {
            InitializeComponent();
            this.mainController = mainController;
        }

        private void FactorsWinBtn_Click(object sender, RoutedEventArgs e)
        {
            MainScope.Content = mainController.CreateFactorsView();
        }

        private void InputWinBtn_Click(object sender, RoutedEventArgs e)
        {
            MainScope.Content = mainController.CreateNewDataView();
        }
    }
}
