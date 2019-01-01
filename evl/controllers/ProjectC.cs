using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using evl.models;

namespace evl.controllers
{
    /// <summary>
    /// ProjectC = Project controls
    /// </summary>
    class ProjectC
    {
        List<Project> projects = new List<Project>();

        public void AddProject(string name, string date, string description)
        {
            Project p = new Project(name, date, description);
            projects.Add(p);
        }

        public void DeleteProject(string name, string date, string description)
        {
            Project p = new Project(name, date, description);
            try {
                projects.Remove(p);
            } catch (Exception ex) {

            }
        }

        public void SaveProjects()
        {
            // В документе не сказано, что именно она должна сохранять
        }
    }
}
