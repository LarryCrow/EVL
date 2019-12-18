using System.Collections.ObjectModel;
using System.ComponentModel;
using evl.Model;

namespace EVL.Model
{
    public interface IReadOnlyViewState : INotifyPropertyChanged
    {
        ReadOnlyObservableCollection<QuestionUI> Questions { get; }

        ReadOnlyObservableCollection<ResultUI> Results { get; }

        string[] QuestionPurposeNames { get; }
    }
}
