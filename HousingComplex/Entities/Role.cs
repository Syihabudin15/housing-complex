using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HousingComplex.Entities
{
    [Table("m_role")]
    public class Role
    {
        [Key, Column(name: "id")] public Guid Id { get; set; }
        [Column(name: "role")] public ERole ERole { get; set; }
    }
}
