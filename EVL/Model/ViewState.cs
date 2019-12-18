using evl.Model;
using EVL.Utils;
using Model;
using Model.Entites;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace EVL.Model
{
    public class ViewState : IReadOnlyViewState
    {
        public ObservableCollection<QuestionUI> Questions { get; }

        public ObservableCollection<ResultUI> Results { get; }
        
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        ReadOnlyObservableCollection<QuestionUI> IReadOnlyViewState.Questions => Questions.AsReadOnly();

        ReadOnlyObservableCollection<ResultUI> IReadOnlyViewState.Results => Results.AsReadOnly();

        public string[] QuestionPurposeNames { get; }

        private ViewState(EvlContext context)
        {
            this.Results = new ObservableCollection<ResultUI>(context.Results.Select(r => new ResultUI { Name = r.Name, Probability = r.Probability }));
            this.Questions = new ObservableCollection<QuestionUI>();
            this.QuestionPurposeNames = Model.QuestionPurposeNames.All;
        }

        public static ViewState RetrieveDataFrom(EvlContext context)
            => new ViewState(context);
    }
}
