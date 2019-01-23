using System.ComponentModel.DataAnnotations;

namespace Model
{
    // оценка клиента
    public class ClientRatingValue
    {
        [Key]
        public int Id { get; set; }
        public double Value { get; set; }

        public int ClientRatingId { get; set; }
        public ClientRating ClientRating { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
