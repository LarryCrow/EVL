using EVL.Utils;
using Model;
using static Model.QuestionPurposeNames;
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
        private readonly ObservableCollection<QuestionUI> questions;

        ReadOnlyObservableCollection<Project> IReadOnlyViewState.Projects => projects.AsReadOnly();
        ReadOnlyObservableCollection<QuestionUI> IReadOnlyViewState.Questions => questions.AsReadOnly();

        public string[] QuestionTypeNames { get; }
        public string[] QuestionPurposeNames { get; }
        public string[] QuestionViewNames { get; }

        private ViewState(DataBaseContext context)
        {
            this.projects = new ObservableCollection<Project>(context.Projects.Take(projectDisplayingCount));
            this.questions = new ObservableCollection<QuestionUI>();

            this.QuestionPurposeNames = context.QuestionPurposes.Select(qp => qp.Name).ToArray();
            this.QuestionTypeNames = context.QuestionTypes.Select(qt => qt.Name).ToArray();
            this.QuestionViewNames = context.QuestionViews.Select(qv => qv.Name).ToArray();
        }

        public static ViewState RetrieveDataFrom(DataBaseContext context)
            => new ViewState(context);

        public void AddProject(Project p)
        {
            if (projects.Count >= projectDisplayingCount)
            {
                projects.RemoveAt(0);
            }

            projects.Add(p);
        }

        public void AddQuestion(QuestionUI q)
        {
            questions.Add(q);
        }

        public void DeleteProject(Project p)
        {
            projects.Remove(p);
        }


        //TODO: move or remove
        public int[] GetClientsIndex()
        {
            return questions
                .Where(q => q.QuestionPurposeName == ClientRating)
                .Select(questions.IndexOf)
                .ToArray();
        }
    }
}
