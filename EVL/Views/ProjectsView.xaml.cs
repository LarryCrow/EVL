using System;
using System.Collections.Generic;
using System.Globalization;
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
using EVL.Model;
using Model;

namespace EVL.Views
{
    /// <summary>
    /// Логика взаимодействия для ProjectsView.xaml
    /// </summary>
    public partial class ProjectsView : UserControl
    {
        private readonly ProjectController controller;
        private readonly IReadOnlyViewState viewState;
        private readonly string[] dateFormats = new[] { "dd.MM.yyyy", "dd/MM/yyyy" };

        public ProjectsView(IReadOnlyViewState viewState, ProjectController controller)
        {
            InitializeComponent();

            this.viewState = viewState;
            this.controller = controller;
            ProjectsTable.ItemsSource = viewState.Projects;
        }

        private void ClearInput()
        {
            TitleInput.Clear();
            DateInput.Clear();
            DescriptionInput.Clear();
        }

        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            var (culture, dtstyle) = (CultureInfo.CurrentUICulture, DateTimeStyles.None);

            if (DateTime.TryParseExact(DateInput.Text, dateFormats, culture, dtstyle, out DateTime date))
            {
                Project p = new Project
                {
                    Name = TitleInput.Text,
                    Description = DescriptionInput.Text,
                    ProjectDate = date
                };

                controller.AddProject(p);
                ClearInput();
            }
            else
            {
                MessageBox.Show("Введите дату в корректном формате.\nНапример: " 
                                + string.Join(" или ", dateFormats));
            }
        }

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            Project p = (Project)ProjectsTable.SelectedItem;

            if (p != null)
                controller.DeleteProject(p);
            else
                MessageBox.Show("Элемент не выбран!");
        }

        private void ChooseProject_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectsTable.SelectedItem != null)
            {
                controller.ChooseProject(((Project)ProjectsTable.SelectedItem).Id);
            }
            else
            {
                MessageBox.Show("Выберите проект");
            }
        }
    }
}
