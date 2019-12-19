using EVL.Model;
using EVL.Views;
using Model;
using System;

namespace EVL.Controllers
{
    public class MainController
    {
        private readonly Func<EvlContext> createDbContext;

        public MainController(Func<EvlContext> createDbContext)
        {
            this.createDbContext = createDbContext;
        }

        public NewDataView CreateNewDataView()
        {
            using (var context = createDbContext())
            {
                var ndvs = new NewDataViewState(context);
                return new NewDataView(ndvs, new NewDataController(ndvs, createDbContext));
            }
        }

        public FactorsView CreateFactorsView()
        {
            using (var context = createDbContext())
            {
                var fvs = new FactorsViewState(context);
                return new FactorsView(fvs, new FactorsController(fvs, createDbContext));
            }
        }
    }
}
