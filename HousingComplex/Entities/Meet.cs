using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingComplex.Entities
{
    [Table(name: "m_meet")]
    public class Meet
    {
        [Key, Column(name: "id")] public Guid Id { get; set; }
        [Column(name: "meet_date")] public string MeetDate { get; set; } = string.Empty;
        [Column(name: "is_meet")] public bool IsMeet { get; set; }
        [Column(name: "housing_id")] public Guid HousingId { get; set; }
        [Column(name: "customer_id")] public Guid CustomerId { get; set; }

        public virtual Housing? Housing { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
