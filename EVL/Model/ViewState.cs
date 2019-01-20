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
        private int currentProjectID;

        public readonly ObservableCollection<Project> Projects;
        public readonly ObservableCollection<QuestionUI> Questions;
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

        private void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        ReadOnlyObservableCollection<Project> IReadOnlyViewState.Projects => Projects.AsReadOnly();
        ReadOnlyObservableCollection<QuestionUI> IReadOnlyViewState.Questions => Questions.AsReadOnly();

        public string[] QuestionPurposeNames { get; }

        private ViewState(DataBaseContext context)
        {
            this.Projects = new ObservableCollection<Project>(context.Projects);
            this.Questions = new ObservableCollection<QuestionUI>();
            this.QuestionPurposeNames = Model.QuestionPurposeNames.All;
            this.currentProjectID = -1;
        }

        public static ViewState RetrieveDataFrom(DataBaseContext context)
            => new ViewState(context);
    }
}
