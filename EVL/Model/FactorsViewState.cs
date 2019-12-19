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

        IEnumerable<Weight> GetWeights(Result result = null, Question question = null);
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
            weights.Add(new Weight { Result = result, Question = question });

        public IEnumerable<Weight> GetWeights(Result result = null, Question question = null)
        {
            var query = weights.AsEnumerable();
            if (result != null) query = weights.Where(w => w.Result?.Id == result.Id || w.ResultId == result.Id);
            if (question != null) query = weights.Where(w => w.Question?.Id == question.Id || w.QuestionId == question.Id);

            return query.ToList();
        }
    }
}
