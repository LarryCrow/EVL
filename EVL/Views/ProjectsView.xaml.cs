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
        private ApplicationModel _model;
        private ProjectC _controller;

        public ProjectsView(ApplicationModel projectModel, ProjectC projectController)
        {
            InitializeComponent();
            _model = projectModel;
            _controller = projectController;
            ProjectsTable.ItemsSource = _model.Projects;
            _model.GetAllProjects();
        }

        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            Project p = new Project();
            p.Name = TitleInput.Text;
            try
            {
                p.ProjectDate = Convert.ToDateTime(DateInput.Text);
            } catch(FormatException ex)
            {
                MessageBox.Show("Введите дату в корректном формате.\nНапример: дд.мм.гггг или дд/мм/ггг");
                return;
            }
            p.Description = DescriptionInput.Text;
            _controller.AddProject(p);
            TitleInput.Text = "";
            DateInput.Text = "";
            DescriptionInput.Text = "";
        }

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            Project p = (Project)ProjectsTable.SelectedItem;
            if (p != null)
                _controller.DeleteProject(p);
            else
                MessageBox.Show("Элемент не выбран!");
        }
    }
}
