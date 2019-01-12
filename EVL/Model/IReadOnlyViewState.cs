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
        ReadOnlyObservableCollection<QuestionUI> Questions { get; }

        string[] QuestionTypeNames { get; }
        string[] QuestionViewNames { get; }
        string[] QuestionPurposeNames { get; }
    }
}
