using EVL.Model;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVL.Controllers
{
    public class NewDataController
    {
        private readonly ViewState viewState;
        private readonly Func<DataBaseContext> createDbContext;

        public NewDataController(ViewState viewState, Func<DataBaseContext> createDbContext)
        {
            this.viewState = viewState;
            this.createDbContext = createDbContext;
        }
    }
}
