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
        public readonly ObservableCollection<MetricQuestionAnswer> metricQA;
        public readonly ObservableCollection<CharacteristicQuestionAnswer> characteristicQA;
        public readonly ObservableCollection<ClientRatingQuestionAnswer> ratingQA;
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
        ReadOnlyObservableCollection<MetricQuestionAnswer> IReadOnlyViewState.MetricQA => metricQA.AsReadOnly();
        ReadOnlyObservableCollection<CharacteristicQuestionAnswer> IReadOnlyViewState.CharacteristicQA => characteristicQA.AsReadOnly();
        ReadOnlyObservableCollection<ClientRatingQuestionAnswer> IReadOnlyViewState.ClientRatingQA => ratingQA.AsReadOnly();

        public string[] QuestionPurposeNames { get; }

        private ViewState(DataBaseContext context)
        {
            this.projects = new ObservableCollection<Project>(context.Projects);
            this.questions = new ObservableCollection<QuestionUI>();
            this.metricQA = new ObservableCollection<MetricQuestionAnswer>();
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
