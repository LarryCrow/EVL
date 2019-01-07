using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EVL.Views;
using Model;

namespace EVL.Controllers
{
    /// <summary>
    /// ProjectC = Project controls
    /// </summary>
    public class ProjectC
    {
        private DataBaseContext _model;

        public ProjectC(DataBaseContext projectModel)
        {
            if (projectModel == null)
                throw new ArgumentNullException("projectModel is null");
            _model = projectModel;
        }

        public void AddProject(Project project)
        {
            _model.Projects.Add(project);
            _model.SaveChanges();
        }

        public void DeleteProject(string name, string date, string description)
        {
        }

        public void SaveProjects()
        {
            // В документе не сказано, что именно она должна сохранять
        }

        public void ShowProjectsView(MainWindow mainWin)
        {
            mainWin.MainScope.Content = new ProjectsView(_model, this);
        }
    }
}
