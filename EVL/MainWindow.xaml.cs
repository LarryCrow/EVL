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
        private readonly IReadOnlyViewState model;
        private readonly ImportController importC;
        private readonly ProjectController projectC;
        private readonly DataBaseController databaseC;
        private readonly FactorsController factorsC;
        private readonly NewDataController newdataC;

        public MainWindow(
            IReadOnlyViewState model,
            ImportController importC,
            ProjectController projectC,
            DataBaseController databaseC,
            FactorsController factorsC,
            NewDataController newdataC)
        {
            InitializeComponent();
            this.model = model;
            this.importC = importC;
            this.projectC = projectC;
            this.databaseC = databaseC;
            this.factorsC = factorsC;
            this.newdataC = newdataC;
        }

        private void ImportWinBtn_Click(object sender, RoutedEventArgs e)
        {
            MainScope.Content = new DataImportView(model, importC);
        }

        private void ProjectsWinBtn_Click(object sender, RoutedEventArgs e)
        {
            MainScope.Content = new ProjectsView(model, projectC);
        }

        private void DBWinBtn_Click(object sender, RoutedEventArgs e)
        {
            MainScope.Content = new DataBaseView(model, databaseC);
        }

        private void FactorsWinBtn_Click(object sender, RoutedEventArgs e)
        {
            MainScope.Content = new FactorsView(model, factorsC);
        }

        private void InputWinBtn_Click(object sender, RoutedEventArgs e)
        {
            MainScope.Content = new NewDataView(model, newdataC);
        }
    }
}
