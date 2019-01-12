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

        ReadOnlyObservableCollection<Project> IReadOnlyViewState.Projects => projects.AsReadOnly();
        ReadOnlyObservableCollection<Question> IReadOnlyViewState.Questions => questions.AsReadOnly();

        public ReadOnlyDictionary<string, QuestionType> QuestionTypes { get; }
        public ReadOnlyDictionary<string, QuestionPurpose> QuestionPurposes { get; }
        public ReadOnlyDictionary<string, QuestionView> QuestionViews { get; }

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


        //TODO: move or remove
        public int[] GetClientsIndex()
        {
            var ratingPurpose = QuestionPurposes[QuestionPurposeNames.ClientRating];

            return questions
                .Where(q => q.QuestionPurpose == ratingPurpose)
                .Select(questions.IndexOf)
                .ToArray();
        }
    }
}
