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
        private readonly IReadOnlyViewState model;

        public MainWindow(IReadOnlyViewState model, MainController mainController)
        {
            InitializeComponent();
            this.mainController = mainController;
            this.model = model;
        }

        private void ImportWinBtn_Click(object sender, RoutedEventArgs e)
        {
            MainScope.Content = mainController.CreateDataImportView();
        }

        private void ProjectsWinBtn_Click(object sender, RoutedEventArgs e)
        {
            MainScope.Content = mainController.CreateProjectsView();
        }

        private void DBWinBtn_Click(object sender, RoutedEventArgs e)
        {
            if (model.CurrentProjectID != -1)
            {
                MainScope.Content = mainController.CreateDataBaseView(model.CurrentProjectID);
            }
            else
            {
                MessageBox.Show("Выберите проект в окне 'Проекты'");
            }
        }

        private void FactorsWinBtn_Click(object sender, RoutedEventArgs e)
        { 
            if (model.CurrentProjectID != -1)
            {
                MainScope.Content = mainController.CreateFactorsView(model.CurrentProjectID);
            }
            else
            {
                MessageBox.Show("Выберите проект в окне 'Проекты'");
            }
        }

        private void InputWinBtn_Click(object sender, RoutedEventArgs e)
        {
            if (model.CurrentProjectID != -1)
            {
                MainScope.Content = mainController.CreateNewDataView(model.CurrentProjectID);
            }
            else
            {
                MessageBox.Show("Выберите проект в окне 'Проекты'");
            }
        }
    }
}
