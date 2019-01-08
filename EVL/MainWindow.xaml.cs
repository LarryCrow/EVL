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

namespace EVL
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProjectC _projectController;
        private ImportController _importController;
        
        public MainWindow(ProjectC projectC, ImportController importC)
        {
            InitializeComponent();
            _projectController = projectC;
            _importController = importC;
        }

        private void ImportWinBtn_Click(object sender, RoutedEventArgs e)
        {
            _importController.ShowImportView(this);
        }

        private void ProjectsWinBtn_Click(object sender, RoutedEventArgs e)
        {
            _projectController.ShowProjectsView(this);
        }

        private void DBWinBtn_Click(object sender, RoutedEventArgs e)
        {
            MainScope.Content = new DataBaseView();
        }

        private void FactorsWinBtn_Click(object sender, RoutedEventArgs e)
        {
            MainScope.Content = new FactorsView();
        }
    }
}
