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

        IEnumerable<Weight> GetWeights(IEnumerable<Result> results = null, IEnumerable<Question> questions = null);
    }

    public class FactorsViewState : IReadOnlyFactorsViewState
    {
        private int resultCounter;
        private int questionCounter;
        private readonly ObservableCollection<Weight> weights;
        private readonly ObservableCollection<Result> results;
        private readonly ObservableCollection<Question> questions;

        public FactorsViewState(EvlContext context)
        {
            results = new ObservableCollection<Result>(context.Results);
            questions = new ObservableCollection<Question>(context.Questions);
            weights = new ObservableCollection<Weight>(context.Weights);
        }

        public ReadOnlyObservableCollection<Result> Results =>
            results.AsReadOnly();

        public ReadOnlyObservableCollection<Question> Questions =>
            questions.AsReadOnly();

        public void AddQuestion() =>
            questions.Add(new Question { Id = -++questionCounter });

        public void AddResult() =>
            results.Add(new Result { Id = -++resultCounter });

        public void AddWeight(Result result, Question question) =>
            weights.Add(new Weight
            {
                ResultId = result.Id,
                Result = result,
                QuestionId = question.Id,
                Question = question
            });

        public IEnumerable<Weight> GetWeights(IEnumerable<Result> results = null, IEnumerable<Question> questions = null)
        {
            var query = weights.AsEnumerable();
            if (results is null || results.Any()) query = query.Where(w => results.Contains(w.Result));
            if (questions is null || questions.Any()) query = query.Where(w => questions.Contains(w.Question));

            return query.ToList();
        }
    }
}
