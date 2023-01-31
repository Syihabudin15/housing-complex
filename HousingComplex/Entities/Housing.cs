using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingComplex.Entities
{
    [Table("m_housing")]
    public class Housing
    {
        [Key, Column(name: "id")] public Guid Id { get; set; }
        [Column(name: "name", TypeName = "Varchar(100)")] public string Name { get; set; } = string.Empty;
        [Column(name: "developer_id")] public Guid DeveloperId { get; set; }
        [Column(name: "address", TypeName = "Varchar(100)")] public string Address { get; set; } = string.Empty;
        [Column(name: "open_time", TypeName = "Varchar(50)")] public string OpenTime { get; set; } = string.Empty;
        [Column(name: "city", TypeName = "50")] public string City { get; set; } = string.Empty;

        public virtual Developer? Developer { get; set; }
    }
}
