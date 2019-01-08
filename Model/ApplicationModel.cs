using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Model
{
    public class ApplicationModel
    {
        private readonly DataBaseContext _context;
        private readonly ObservableCollection<Project> projects;

        public ApplicationModel(DataBaseContext context)
        {
            this._context = context;
            this.projects = new ObservableCollection<Project>();
        }

        public ReadOnlyObservableCollection<Project> Projects
            => new ReadOnlyObservableCollection<Project>(projects);

        public void AddProject(Project p)
        {
            _context.Projects.Add(p);
            _context.SaveChanges();
            if (Projects.Count >= 5)
            {
                projects.RemoveAt(0);
            }
            
            projects.Add(p);
        }

        public void DeleteProject(Project p)
        {
            _context.Projects.Remove(p);
            _context.SaveChanges();
            if (projects.Count > 0)
            {
                projects.Remove(p);
            }
        }

        public void GetAllProjects()
        {
            var projectItems = _context.Projects;
            foreach(Project pr in projectItems)
            {
                projects.Add(pr);
            }
        }
    }
}
