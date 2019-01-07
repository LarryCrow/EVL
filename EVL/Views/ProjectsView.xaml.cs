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
using EVL.Controllers;
using Model;

namespace EVL.Views
{
    /// <summary>
    /// Логика взаимодействия для ProjectsView.xaml
    /// </summary>
    public partial class ProjectsView : UserControl
    {
        private DataBaseContext _model;
        private ProjectC _controller;

        public ProjectsView(DataBaseContext projectModel, ProjectC projectController)
        {
            InitializeComponent();
            _model = projectModel;
            _controller = projectController;
        }

        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            Project p = new Project();
            p.Name = TitleInput.Text;
            p.ProjectDate = Convert.ToDateTime(DateInput.Text);
            p.Description = DescriptionInput.Text;
            _controller.AddProject(p);
        }

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
