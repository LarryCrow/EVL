using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace EVL.Model
{
    public interface IReadOnlyViewState
    {
        ReadOnlyObservableCollection<Project> Projects { get; }
        ReadOnlyObservableCollection<Question> Questions { get; }

        ReadOnlyDictionary<string, QuestionType> QuestionTypes { get; }
        ReadOnlyDictionary<string, QuestionPurpose> QuestionPurposes { get; }
        ReadOnlyDictionary<string, QuestionView> QuestionViews { get; }
    }
}
