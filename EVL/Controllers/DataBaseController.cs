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
        private readonly DataBaseContext context;
        private readonly ViewState viewState;

        public DataBaseController(ViewState viewState, DataBaseContext context)
        {
            this.context = context;
            this.viewState = viewState;
        }
    }
}
