using System;
using System.Collections.Generic;
using System.Linq;
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

        public void AddWeight(Result result, Question question) =>
            viewState.AddWeight(result, question);

        public void SubmitChanges()
        {
            // Disable two-way binding configurations to suppress ChangeTracker issue on last step.
            foreach (var result in viewState.Results) result.Weights = null;

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
            }
        }

        private static void SynchronizeWithCollection<T>(
            DbSet<T> table,
            IEnumerable<T> collection,
            Func<T, T, bool> identityComparer,
            Action<T> setDefaultKeyOnAdd = null) where T : class
        {
            var source = table.ToList();
            var difference = CollectionUtils.Diff(source, collection, identityComparer);

            table.RemoveRange(difference.Updated
                .Select(items => items.Source)
                .Concat(difference.Removed)
            );

            table.AddRange(difference.Updated
                .Select(items => items.Target)
                .Concat(difference.Added.Select(e => { setDefaultKeyOnAdd?.Invoke(e); return e; }))
            );
        }
    }
}
