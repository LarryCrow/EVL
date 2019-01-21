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
using EVL.Views;
using EVL.Controllers;
using Model;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
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
                MainScope.Content = mainController.CreateDataBaseView();
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
