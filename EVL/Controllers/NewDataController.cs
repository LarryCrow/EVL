using EVL.Model;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EVL.Controllers
{
    public class NewDataController
    {
        private readonly NewDataViewState viewState;
        private readonly Func<EvlContext> createDbContext;

        public NewDataController(NewDataViewState viewState, Func<EvlContext> createDbContext)
        {
            this.viewState = viewState;
            this.createDbContext = createDbContext;
        }

        public ResultProbabilityUI[] Calculate()
        {
            using (var context = createDbContext())
            {
                var initialResults = context.Results.Include(r => r.Weights).Select(r => new ResultProbabilityUI { Result = r, ConditionalProbability = r.Probability }).ToArray();
                return viewState.QAList.Aggregate(initialResults, CalculatePosteriorProbablilities);
            }
        }

        private static ResultProbabilityUI[] CalculatePosteriorProbablilities(ResultProbabilityUI[] hypotheses, QuestionAnswerUI qa)
        {
            double CalculatePosteriorProbablilityForHypothesis(double p, Weight qw) => qa.Yes
                ? qw.PPlus * p / (qw.PPlus * p + qw.PMinus * (1 - p))
                : (1 - qw.PPlus) * p / ((1 - qw.PPlus) * p + (1 - qw.PMinus) * (1 - p));

            return Array.ConvertAll(hypotheses, h =>
                h.Result.Weights?.SingleOrDefault(w => w.QuestionId == qa.Question.Id) is Weight qw
                ? new ResultProbabilityUI
                {
                    Result = h.Result,
                    ConditionalProbability = CalculatePosteriorProbablilityForHypothesis(h.ConditionalProbability, qw),
                }
                : h
            );
        }
    }
}
