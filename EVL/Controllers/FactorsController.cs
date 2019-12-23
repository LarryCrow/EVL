using System;
using System.Collections.Generic;
using System.Linq;
using evl.Model;
using EVL.Model;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Entites;
using Saritasa.Tools.Common.Utils;

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
            // Disable two-way binding configurations to suppress ChangeTracker issue on last step.
            foreach (var result in viewState.Results) result.Value.Weights = null;

            using (var context = dbContextFactory.Invoke())
            {
                SynchronizeWithCollection(context.Results, viewState.Results,
                    (s, t) => !t.Deleted && s.Id == t.Value.Id, r => { r.Id = default; });

                SynchronizeWithCollection(context.Questions, viewState.Questions,
                    (s, t) => !t.Deleted && s.Id == t.Value.Id, q => q.Id = default);

                SynchronizeWithCollection(context.Weights, viewState.GetWeights(),
                    (s, t) => !t.Deleted && s.ResultId == t.Value.ResultId && s.QuestionId == t.Value.QuestionId,
                    w => { w.ResultId = default; w.QuestionId = default; });

                context.SaveChanges();
            }
        }

        internal void SoftDelete(IEnumerable<DeletableUI<Result>> enumerable1, IEnumerable<DeletableUI<Question>> enumerable2, IEnumerable<DeletableUI<Weight>> enumerable3)
        {
            throw new NotImplementedException();
        }

        private static void SynchronizeWithCollection<T>(
            DbSet<T> table,
            IEnumerable<DeletableUI<T>> collection,
            Func<T, DeletableUI<T>, bool> identityComparer,
            Action<T> setDefaultKeyOnAdd = null) where T : class
        {
            var source = table.ToList();
            var difference = CollectionUtils.Diff(source.Select(v => new DeletableUI<T> { Value  = v }), collection, (s,t) => identityComparer(s.Value, t));

            table.RemoveRange(difference.Updated
                .Select(items => items.Source)
                .Concat(difference.Removed)
                .Select(d => d.Value)
            );

            table.AddRange(difference.Updated
                .Select(items => items.Target)
                .Concat(difference.Added.Select(e => { setDefaultKeyOnAdd?.Invoke(e.Value); return e; }))
                .Select(d => d.Value)
            );
        }
    }
}
