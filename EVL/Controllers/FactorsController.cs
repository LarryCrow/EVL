using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVL.Model;
using Model;

namespace EVL.Controllers
{
    public class FactorsController
    {
        private readonly ViewState viewState;
        private readonly Func<DataBaseContext> createDbContext;

        public FactorsController(ViewState viewState, Func<DataBaseContext> createDbContext)
        {
            this.viewState = viewState;
            this.createDbContext = createDbContext;
        }
    }
}
