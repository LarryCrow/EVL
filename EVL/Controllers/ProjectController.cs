using System;
using EVL.Model;
using Model;

namespace EVL.Controllers
{
    /// <summary>
    /// ProjectController = Project controls
    /// </summary>
    public class ProjectController
    {
        private readonly Func<EvlContext> createDbContext;
        private readonly ViewState viewState;

        public ProjectController(ViewState viewState, Func<EvlContext> createDbContext)
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

            viewState.Projects.Add(p);
        }

        public void DeleteProject(Project p)
        {
            using (var context = createDbContext())
            {
                context.Projects.Remove(p);
                context.SaveChanges();
            }

            viewState.Projects.Remove(p);
        }

        public void ChooseProject(int projectID)
        {
            viewState.CurrentProjectID = projectID;
        }
    }
}
