using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.Entites
{
    public class Result
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public double Probability { get; set; }

        public string Description { get; set; }

        public ICollection<Weight> Weights { get; set; }
    }
}
