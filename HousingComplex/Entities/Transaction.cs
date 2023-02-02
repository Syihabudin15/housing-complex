using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingComplex.Entities
{
    [Table("t_transaction")]
    public class Transaction
    {
        [Key, Column(name: "id")] public Guid Id { get; set; }
        [Column(name: "trans_date")] public DateTime TransDate { get; set; }
        [Column(name: "customer_id")] public Guid CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }
        
        public virtual TransactionDetail? TransactionDetail { get; set; }
    }
}
