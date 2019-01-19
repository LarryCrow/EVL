using EVL.Utils;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EVL.Model
{
    public class ViewState : IReadOnlyViewState
    {
        public readonly ObservableCollection<Project> projects;
        public readonly ObservableCollection<QuestionUI> questions;
        public readonly ObservableCollection<QuestionAnswers> questionAnswers;
        private int currentProjectID;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public int CurrentProjectID
        {
            get { return currentProjectID; }
            set
            {
                currentProjectID = value;
                OnPropertyChanged(nameof(CurrentProjectID));
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        ReadOnlyObservableCollection<Project> IReadOnlyViewState.Projects => projects.AsReadOnly();
        ReadOnlyObservableCollection<QuestionUI> IReadOnlyViewState.Questions => questions.AsReadOnly();
        ReadOnlyObservableCollection<QuestionAnswers> IReadOnlyViewState.QuestionAnswers => questionAnswers.AsReadOnly();

        public string[] QuestionPurposeNames { get; }

        private ViewState(DataBaseContext context)
        {
            this.projects = new ObservableCollection<Project>(context.Projects);
            this.questions = new ObservableCollection<QuestionUI>();
            this.questionAnswers = new ObservableCollection<QuestionAnswers>();
            this.QuestionPurposeNames = Model.QuestionPurposeNames.All;
            this.currentProjectID = -1;
        }

        public static ViewState RetrieveDataFrom(DataBaseContext context)
            => new ViewState(context);

        //TODO: move or remove
        public int[] GetClientsIndex()
        {
            return questions
                .Where(q => q.QuestionPurposeName == Model.QuestionPurposeNames.ClientRating)
                .Select(questions.IndexOf)
                .ToArray();
        }
    }
}
