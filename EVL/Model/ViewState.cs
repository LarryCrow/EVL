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
        private readonly int projectDisplayingCount = 10;
        public readonly ObservableCollection<Project> projects;
        public readonly ObservableCollection<QuestionUI> questions;
        private int currentProjectID;
        public event PropertyChangedEventHandler PropertyChanged;
        public int CurrentProjectID
        {
            get { return currentProjectID; }
            set
            {
                currentProjectID = value;
                OnPropertyChanged("CurrentProjectID");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        ReadOnlyObservableCollection<Project> IReadOnlyViewState.Projects => projects.AsReadOnly();
        ReadOnlyObservableCollection<QuestionUI> IReadOnlyViewState.Questions => questions.AsReadOnly();

        public string[] QuestionPurposeNames { get; }

        private ViewState(DataBaseContext context)
        {
            this.projects = new ObservableCollection<Project>(context.Projects.Take(projectDisplayingCount));
            this.questions = new ObservableCollection<QuestionUI>();
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
