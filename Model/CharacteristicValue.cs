using System.ComponentModel.DataAnnotations;

namespace Model
{
    // название клиента
    public class CharacteristicValue
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }

        public int CharacteristicId { get; set; }
        public Characteristic Characteristic { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
