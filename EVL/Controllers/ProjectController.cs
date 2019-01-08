using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EVL.Model;
using EVL.Views;
using Model;

namespace EVL.Controllers
{
    /// <summary>
    /// ProjectController = Project controls
    /// </summary>
    public class ProjectController
    {
        private readonly DataBaseContext context;
        private readonly ViewState viewState;

        public ProjectController(ViewState viewState, DataBaseContext context)
        {
            this.context = context;
            this.viewState = viewState;
        }

        public void AddProject(Project p)
        {
            context.Projects.Add(p);
            context.SaveChanges();

            viewState.AddProject(p);
        }

        public void DeleteProject(Project p)
        {
            context.Projects.Remove(p);
            context.SaveChanges();

            viewState.DeleteProject(p);
        }
    }
}
