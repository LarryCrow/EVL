﻿using System;
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
        private readonly Func<DataBaseContext> createDbContext;
        private readonly ViewState viewState;

        public ProjectController(ViewState viewState, Func<DataBaseContext> createDbContext)
        {
            this.createDbContext = createDbContext;
            this.viewState = viewState;
        }

        public void AddProject(Project p)
        {
            using (var context = createDbContext())
            {
                context.Projects.Add(p);
                context.SaveChanges();
            }

            viewState.projects.Add(p);
        }

        public void DeleteProject(Project p)
        {
            using (var context = createDbContext())
            {
                context.Projects.Remove(p);
                context.SaveChanges();
            }

            viewState.projects.Remove(p);
        }

        public void ChooseProject(int projectID)
        {
            viewState.CurrentProjectID = projectID;
        }
    }
}
