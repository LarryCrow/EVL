using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.ComponentModel;

namespace EVL.Model
{
    public interface IReadOnlyViewState : INotifyPropertyChanged
    {
        ReadOnlyObservableCollection<Project> Projects { get; }
        ReadOnlyObservableCollection<QuestionUI> Questions { get; }
        ReadOnlyObservableCollection<MetricQuestionAnswer> MetricQA { get; }
        ReadOnlyObservableCollection<CharacteristicQuestionAnswer> CharacteristicQA { get; }
        ReadOnlyObservableCollection<ClientRatingQuestionAnswer> ClientRatingQA { get; }
        int CurrentProjectID { set; get; }

        string[] QuestionPurposeNames { get; }
    }
}
