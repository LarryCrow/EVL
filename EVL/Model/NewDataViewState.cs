using System.Collections.ObjectModel;
using System.Linq;
using EVL.Utils;
using Model;

namespace EVL.Model
{
    public interface IReadOnlyNewDataViewState
    {
        ReadOnlyObservableCollection<QuestionAnswerUI> QAList { get; }
    }

    public class NewDataViewState : IReadOnlyNewDataViewState
    {
        private readonly ObservableCollection<QuestionAnswerUI> qaList;

        public NewDataViewState(EvlContext context)
        {
            qaList = new ObservableCollection<QuestionAnswerUI>(context.Questions.Select(q => new QuestionAnswerUI { Question = q }));
        }

        public ReadOnlyObservableCollection<QuestionAnswerUI> QAList => qaList.AsReadOnly();
    }
}
