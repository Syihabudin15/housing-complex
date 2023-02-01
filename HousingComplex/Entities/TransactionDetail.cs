using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingComplex.Entities
{
    [Table(name: "t_transaction_detail")]
    public class TransactionDetail
    {
        [Key, Column(name: "id")] public Guid Id { get; set; }
        [Column(name: "house_type_id")] public Guid HouseTypeId { get; set; }
        [Column(name: "transaction_id")] public Guid TransactionId { get; set; }
        [Column(name: "housing_id")] public Guid HousingId { get; set; }
        [Column(name: "reference_pg")] public string ReferencePg { get; set; } = string.Empty;
        [Column(name: "nominal")] public long Nominal { get; set; }
        [Column(name: "description")] public string Description { get; set; } = string.Empty;
        [Column(name: "is_paid")] public bool IsPaid { get; set; }
        [Column(name: "order_id")] public string OrderId { get; set; }

        public virtual HouseType? HouseType { get; set; }
        public virtual Transaction? Transaction { get; set; }
        public virtual Housing? Housing { get; set; }
    }
}
