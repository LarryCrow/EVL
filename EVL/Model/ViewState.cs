using EVL.Utils;
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
        private readonly ObservableCollection<Question> questions;

        private ViewState(DataBaseContext context)
        {
            this.projects = new ObservableCollection<Project>(context.Projects.Take(projectDisplayingCount));
            this.questions = new ObservableCollection<Question>();

            this.QuestionPurposes = context.QuestionPurposes.ToDictionary(qp => qp.Name).AsReadOnly();
            this.QuestionTypes = context.QuestionTypes.ToDictionary(qt => qt.Name).AsReadOnly();
            this.QuestionViews = context.QuestionViews.ToDictionary(qv => qv.Name).AsReadOnly();
        }

        public static ViewState RetrieveDataFrom(DataBaseContext context)
            => new ViewState(context);

        ReadOnlyObservableCollection<Project> IReadOnlyViewState.Projects
            => new ReadOnlyObservableCollection<Project>(projects);

        ReadOnlyObservableCollection<Question> IReadOnlyViewState.Questions
            => new ReadOnlyObservableCollection<Question>(questions);

        public ReadOnlyDictionary<string, QuestionType> QuestionTypes { get; }
        public ReadOnlyDictionary<string, QuestionPurpose> QuestionPurposes { get; }
        public ReadOnlyDictionary<string, QuestionView> QuestionViews { get; }

        public void AddProject(Project p)
        {
            if (projects.Count >= projectDisplayingCount)
            {
                projects.RemoveAt(0);
            }

            projects.Add(p);
        }

        public void AddQuestion(Question q)
        {
            questions.Add(q);
        }

        public void DeleteProject(Project p)
        {
            projects.Remove(p);
        }

        public string GetSegmentName()
        {
            Question question = questions.Where(q => q.QuestionPurposeId == 3).First();
            return question.Name;
        }

        public int[] GetClientsIndex()
        {
            IEnumerable<Question> questions = this.questions.Where(q => q.QuestionPurposeId == 2);
            List<int> indexes = new List<int>();
            foreach(Question q in questions)
            {
                indexes.Add(this.questions.IndexOf(q));
            }
            return indexes.ToArray();
        }

        public IEnumerable<Question> GetQuestions()
        {
            return questions.Where(q => q.QuestionPurposeId != 3 && q.QuestionPurposeId != 4);
        }
    }
}
