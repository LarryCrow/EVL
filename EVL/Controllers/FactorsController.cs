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
        private readonly DataBaseContext context;
        private readonly ViewState viewState;

        public FactorsController(ViewState viewState, DataBaseContext context)
        {
            this.context = context;
            this.viewState = viewState;
        }
    }
}
