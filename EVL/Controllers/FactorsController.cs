using EVL.Model;
using Model;
using System;
using System.Linq;

namespace EVL.Controllers
{
    public class FactorsController
    {
        private readonly FactorsViewState viewState;
        private readonly Func<EvlContext> createDbContext;

        public FactorsController(FactorsViewState viewState, Func<EvlContext> createDbContext)
        {
            this.viewState = viewState;
            this.createDbContext = createDbContext;
        }

        public void AddQuestion() => viewState.AddQuestion();

        public void AddResult() => viewState.AddResult();

        public void SubmitChanges()
        {
            using (var context = createDbContext())
            {
                context.Results.AddRange(viewState.Results.Where(r => r.Id < 0));
                context.Questions.AddRange(viewState.Questions.Where(q => q.Id < 0));

                foreach (var (isNew, weight) in viewState.GetWeights())
                {
                    if (weight.Result?.Id > 0)
                    {
                        context.Attach(weight.Result);
                    }

                    if (weight.Question?.Id > 0)
                    {
                        context.Attach(weight.Question);
                    }

                    if (isNew)
                    {
                        context.Add(weight);
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
