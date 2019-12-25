using EVL.Model;
using EVL.Utils;
using Model;
using Model.Entites;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EVL.Model
{
    public interface IReadOnlyFactorsViewState
    {
        ReadOnlyObservableCollection<DeletableUI<Question>> Questions { get; }

        ReadOnlyObservableCollection<DeletableUI<Result>> Results { get; }

        IEnumerable<DeletableUI<Weight>> GetWeights(IEnumerable<DeletableUI<Result>> results = null, IEnumerable<DeletableUI<Question>> questions = null);
    }

    public class FactorsViewState : IReadOnlyFactorsViewState
    {
        private int resultCounter;
        private int questionCounter;
        private readonly ObservableCollection<DeletableUI<Weight>> weights;
        private readonly ObservableCollection<DeletableUI<Result>> results;
        private readonly ObservableCollection<DeletableUI<Question>> questions;

        public FactorsViewState(EvlContext context)
        {
            results = new ObservableCollection<DeletableUI<Result>>(context.Results.Select(r => new DeletableUI<Result> { Value = r }));
            questions = new ObservableCollection<DeletableUI<Question>>(context.Questions.Select(q => new DeletableUI<Question> { Value = q }));
            weights = new ObservableCollection<DeletableUI<Weight>>(context.Weights.Select(w => new DeletableUI<Weight> { Value = w }));
        }

        public ReadOnlyObservableCollection<DeletableUI<Result>> Results =>
            results.AsReadOnly();

        public ReadOnlyObservableCollection<DeletableUI<Question>> Questions =>
            questions.AsReadOnly();

        public void AddQuestion() =>
            questions.Add(new DeletableUI<Question> { Value = new Question { Id = -++questionCounter } });

        public void AddResult() =>
            results.Add(new DeletableUI<Result> { Value = new Result { Id = -++resultCounter } });

        public void AddWeight(DeletableUI<Result> result, DeletableUI<Question> question) =>
            weights.Add(new DeletableUI<Weight>
            {
                CascadeDeleted = result.Deleted || question.Deleted,
                Value = new Weight
                {
                    ResultId = result.Value.Id,
                    Result = result.Value,
                    QuestionId = question.Value.Id,
                    Question = question.Value
                }
            });

        public IEnumerable<DeletableUI<Weight>> GetWeights(IEnumerable<DeletableUI<Result>> results = null,
            IEnumerable<DeletableUI<Question>> questions = null)
        {
            var query = weights.AsEnumerable();

            if (results != null)
                query = query.Where(w => results.Any(r => r.Value == w.Value.Result));

            if (questions != null) 
                query = query.Where(w => questions.Any(q => q.Value == w.Value.Question));

            return query.ToList();
        }

        public void ClearDeletedObjects()
        {
            foreach (var r in results.Where(r => r.Deleted).ToList()) results.Remove(r);
            foreach (var q in questions.Where(r => r.Deleted).ToList()) questions.Remove(q);
            foreach (var w in weights.Where(r => r.Deleted).ToList()) weights.Remove(w);
        }
    }
}
