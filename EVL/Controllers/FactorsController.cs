using System;
using System.Collections.Generic;
using System.Linq;
using EVL.Model;
using Microsoft.EntityFrameworkCore;
using Model;
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

        public void SubmitChanges()
        {
            using (var context = dbContextFactory.Invoke())
            {
                SynchronizeWithCollection(context.Results, viewState.Results,
                    (s, t) => s.Id == t.Id, t => t.Id = default);
                
                SynchronizeWithCollection(context.Questions, viewState.Questions,
                    (s, t) => s.Id == t.Id, t => t.Id = default);

                SynchronizeWithCollection(context.Weights, viewState.GetWeights(),
                    (s, t) => s.ResultId == t.ResultId && s.QuestionId == t.QuestionId);
                
                context.SaveChanges();
            }
        }

        private static void SynchronizeWithCollection<T>(
            DbSet<T> table,
            IEnumerable<T> collection,
            Func<T, T, bool> identityComparer,
            Action<T> beforeUpdate = null) where T : class
        {
            var difference = CollectionUtils.Diff(table, collection, identityComparer);

            table.AddRange(difference.Added);
            table.RemoveRange(difference.Removed);
            table.AttachRange(difference.Updated.Select(items => { beforeUpdate?.Invoke(items.Target); return items.Target; }));
        }
    }
}
