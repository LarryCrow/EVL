using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EVL.Model
{
    public class ViewState : IReadOnlyViewState
    {
        private readonly int projectDisplayingCount = 10;
        private readonly ObservableCollection<Project> projects;

        private ViewState(DataBaseContext context)
        {
            this.projects = new ObservableCollection<Project>(context.Projects.Take(projectDisplayingCount));
        }

        public static ViewState RetrieveDataFrom(DataBaseContext context)
            => new ViewState(context);

        ReadOnlyObservableCollection<Project> IReadOnlyViewState.Projects
            => new ReadOnlyObservableCollection<Project>(projects);

        public void AddProject(Project p)
        {
            if (projects.Count >= projectDisplayingCount)
            {
                projects.RemoveAt(0);
            }
            
            projects.Add(p);
        }

        public void DeleteProject(Project p)
        {
            projects.Remove(p);
        }
    }
}
