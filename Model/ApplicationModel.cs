using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Model
{
    public class ApplicationModel
    {
        private readonly DataBaseContext _context;
        private readonly ObservableCollection<Project> projects;
        public List<Question> tempQuestions;
        public ObservableCollection<Question> questions;

        public ApplicationModel(DataBaseContext context)
        {
            this._context = context;
            this.projects = new ObservableCollection<Project>();
            this.tempQuestions = new List<Question>();
            this.questions = new ObservableCollection<Question>();
        }

        public ReadOnlyObservableCollection<Project> Projects
            => new ReadOnlyObservableCollection<Project>(projects);


        #region Project methods

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

        #endregion

        #region Import methods

        public void AddIntoTemporaryList(Question q)
        {
            tempQuestions.Add(q);
        }

        #endregion


        #region Question

        public QuestionType GetQuestionType(string name)
        {
            return _context.QuestionTypes.Single(qt => qt.Name == name);
        }

        public QuestionView GetQuestionView(string name)
        {
            return _context.QuestionViews.Single(qt => qt.Name == name);
        }

        public QuestionPurpose GetQuestionPurpose(string name)
        {
            return _context.QuestionPurposes.Single(qt => qt.Name == name);
        }

        public QuestionType[] GetAllQuestionType()
        {
            return _context.QuestionTypes.ToArray();
        }

        public QuestionView[] GetAllQuestionView()
        {
            return _context.QuestionViews.ToArray();
        }

        public QuestionPurpose[] GetAllQuestionPurpose()
        {
            return _context.QuestionPurposes.ToArray();
        }

        #endregion
    }
}
