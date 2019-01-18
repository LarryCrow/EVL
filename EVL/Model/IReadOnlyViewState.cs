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
        
        string[] QuestionPurposeNames { get; }
    }
}
