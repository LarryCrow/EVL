using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evl.models
{
    class Project
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }

        public Project(string name, string date, string description)
        {
            this.Name = name;
            this.Date = date;
            this.Description = description;
        }
    }
}
