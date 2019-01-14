using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using EVL.Model;

namespace EVL.Controllers
{
    public class DataBaseController
    {
        private readonly ViewState viewState;
        private readonly Func<DataBaseContext> createDbContext;

        public DataBaseController(ViewState viewState, Func<DataBaseContext> createDbContext)
        {
            this.viewState = viewState;
            this.createDbContext = createDbContext;
        }
    }
}
