using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    // оценка клиента
    public class ClientRating
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public double Weight { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        IEnumerable<ClientRatingValue> ClientRatingValues { get; set; }
    }
}
