using System;
using System.Collections.Generic;
using System.Linq;
using EVL.Model;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Entites;
using Saritasa.Tools.Common.Utils;
using Saritasa.Tools.Domain.Exceptions;

namespace EVL.Controllers
{
    public class FactorsController
    {
        private readonly FactorsViewState viewState;
        private readonly Func<EvlContext> dbContextFactory;

        public FactorsController(FactorsViewState viewState, Func<EvlContext> createDbContext)
        {
            this.viewState = viewState;
            this.dbContextFactory = createDbContext;
        }

        public void AddQuestionToState() =>
            viewState.AddQuestion();

        public void AddResultToState() =>
            viewState.AddResult();

        public void AddWeight(DeletableUI<Result> result, DeletableUI<Question> question) =>
            viewState.AddWeight(result, question);

        public void SubmitChanges()
        {
            ThrowFromViewStateValidation(viewState);

            // Disable two-way binding configurations to suppress ChangeTracker issue on last step.
            foreach (var result in viewState.Results) result.Value.Weights = null;

            using (var context = dbContextFactory.Invoke())
            {
                SynchronizeWithCollection(context.Results, viewState.Results,
                    (s, t) => s.Id == t.Id, r => { r.Id = default; });

                SynchronizeWithCollection(context.Questions, viewState.Questions,
                    (s, t) => s.Id == t.Id, q => q.Id = default);

                SynchronizeWithCollection(context.Weights, viewState.GetWeights(),
                    (s, t) => s.ResultId == t.ResultId && s.QuestionId == t.QuestionId,
                    w => { w.ResultId = default; w.QuestionId = default; });

                context.SaveChanges();
                viewState.ClearDeletedObjects();
            }
        }

        internal void ChangeEntitiesState(IEnumerable<DeletableUI<Result>> results,
            IEnumerable<DeletableUI<Question>> questions,
            IEnumerable<DeletableUI<Weight>> weights,
            bool deleted)
        {
            foreach (var r in results)
                r.ForceDeleted = deleted;

            foreach (var q in questions)
                q.ForceDeleted = deleted;

            foreach (var w in weights)
                w.ForceDeleted = deleted;

            foreach (var w in Enumerable.Union(viewState.GetWeights(results: results ?? new DeletableUI<Result>[0]),
                                               viewState.GetWeights(questions: questions ?? new DeletableUI<Question>[0])))
                w.CascadeDeleted = deleted;
        }


        private static void SynchronizeWithCollection<T>(
            DbSet<T> table,
            IEnumerable<DeletableUI<T>> collection,
            Func<T, T, bool> identityComparer,
            Action<T> setDefaultKeyOnAdd = null) where T : class
        {
            var source = table.ToList();
            var difference = CollectionUtils.Diff(source, collection.ExcludeDeleted(), identityComparer);

            table.RemoveRange(difference.Updated
                .Select(items => items.Source)
                .Concat(difference.Removed)
            );

            table.AddRange(difference.Updated
                .Select(items => items.Target)
                .Concat(difference.Added.Select(e => { setDefaultKeyOnAdd?.Invoke(e); return e; }))
            );
        }

        private static void ThrowFromViewStateValidation(IReadOnlyFactorsViewState viewState)
        {
            var exception = new ValidationException();

            if (viewState.Results.ExcludeDeleted().Sum(q => q.Probability) is var sum && sum != 1.0)
            {
                exception.AddError("Results", $"Sum of probabilities {sum} is not equal to 1 as expected.");
            }

            foreach (var q in viewState.Questions.ExcludeDeleted())
            {
                if (string.IsNullOrWhiteSpace(q.Property) || string.IsNullOrWhiteSpace(q.QuestionText))
                    exception.AddError("Questions", $"Q{q.Id:#;\\##;0} - no title or description");
            }

            foreach (var r in viewState.Results.ExcludeDeleted())
            {
                if (r.Probability < 0 || 1 < r.Probability)
                    exception.AddError("Results", $"R{r.Id:#;\\##;0} - probability is out of range [0;1]");

                else if (string.IsNullOrWhiteSpace(r.Description) || string.IsNullOrWhiteSpace(r.Name))
                    exception.AddError("Results", $"R{r.Id:#;\\##;0} - no name or description");
            }
            
            foreach (var w in viewState.GetWeights().ExcludeDeleted())
            {
                if (w.PMinus < 0 || 1 < w.PMinus || w.PPlus < 0 || 1 < w.PPlus)
                    exception.AddError("Weights", $"(R{w.ResultId:#;\\##;0}, Q{w.QuestionId:#;\\##;0}) - one of the probabilities is out of range [0;1]");
            }

            if (exception.Errors.Any())
            {
                throw exception;
            }
        }
    }
}
