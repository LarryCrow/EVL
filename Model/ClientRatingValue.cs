using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
