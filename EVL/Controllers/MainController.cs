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
            var ndvs = new NewDataViewState();
            return new NewDataView(ndvs, new NewDataController(ndvs, createDbContext));
        }

        public FactorsView CreateFactorsView()
        {
            var fvs = new FactorsViewState(createDbContext());
            return new FactorsView(fvs, new FactorsController(fvs, createDbContext));
        }
    }
}
