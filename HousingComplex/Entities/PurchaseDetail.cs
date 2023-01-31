using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingComplex.Entities
{
    [Table(name: "t_purchase_detail")]
    public class PurchaseDetail
    {
        [Key, Column(name: "id")] public Guid Id { get; set; }
        [Column(name: "house_type_id")] public Guid HouseTypeId { get; set; }
        [Column(name: "purchase_id")] public Guid PurchaseId { get; set; }
        [Column(name: "housing_id")] public Guid HousingId { get; set; }
        [Column(name: "reference_pg")] public string ReferencePg { get; set; } = string.Empty;
        [Column(name: "nominal")] public long Nominal { get; set; }
        [Column(name: "description")] public string Description { get; set; } = string.Empty;

        public virtual HouseType? HouseType { get; set; }
        public virtual Purchase? Purchase { get; set; }
        public virtual Housing? Housing { get; set; }
    }
}
