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
        private ApplicationModel _model;

        public ProjectC(ApplicationModel projectModel)
        {
            if (projectModel == null)
                throw new ArgumentNullException("projectModel is null");
            _model = projectModel;
        }

        public void AddProject(Project project)
        {
            _model.AddProject(project);
        }

        public void DeleteProject(Project project)
        {
            _model.DeleteProject(project);
        }

        public void ShowProjectsView(MainWindow mainWin)
        {
            mainWin.MainScope.Content = new ProjectsView(_model, this);
        }
    }
}
