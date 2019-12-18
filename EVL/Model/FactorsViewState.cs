using EVL.Utils;
using Model;
using Model.Entites;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EVL.Model
{
    public interface IReadOnlyFactorsViewState
    {
        ReadOnlyObservableCollection<Question> Questions { get; }
        ReadOnlyObservableCollection<Result> Results { get; }
        IEnumerable<(bool isNew, Weight)> GetWeights(Result result = null, Question question = null);
    }

    public class FactorsViewState : IReadOnlyFactorsViewState
    {
        private int resultCounter;
        private int questionCounter;
        private readonly ObservableCollection<(bool isNew, Weight)> weights;
        private readonly ObservableCollection<Result> results;
        private readonly ObservableCollection<Question> questions;

        public FactorsViewState(EvlContext context)
        {
            results = new ObservableCollection<Result>(context.Results);
            questions = new ObservableCollection<Question>(context.Questions);
            weights = new ObservableCollection<(bool isNew, Weight)>(context.Weights.ToList().Select(w => (false, w)));
        }

        public void AddQuestion() => questions.Add(new Question { Id = -++questionCounter });

        public void AddResult() => results.Add(new Result { Id = -++resultCounter });

        public void AddWeight(Result result, Question question) => 
            weights.Add((true, new Weight
            {
                Result = result,
                Question = question,
            }));

        public IEnumerable<(bool isNew, Weight)> GetWeights(Result result = null, Question question = null)
        {
            var query = weights.AsEnumerable();
            if (result != null) query = weights.Where(t => t.isNew ? t.Item2.Result == result : t.Item2.ResultId == result.Id);
            if (question != null) query = weights.Where(t => t.isNew ? t.Item2.Question == question : t.Item2.QuestionId == question.Id);

            return query.ToList();
        }

        public ReadOnlyObservableCollection<Result> Results => results.AsReadOnly();

        public ReadOnlyObservableCollection<Question> Questions => questions.AsReadOnly();
    }
}
