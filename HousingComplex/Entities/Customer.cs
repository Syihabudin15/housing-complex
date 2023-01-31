using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingComplex.Entities
{
    [Table("m_customer")]
    public class Customer
    {
        [Key, Column(name: "id")] public Guid Id { get; set; }
        [Column(name: "first_name", TypeName = "Varchar(50)")] public string FisrtName { get; set; } = string.Empty;
        [Column(name: "last_name", TypeName = "Varchar(50)")] public string LastName { get; set; } = string.Empty;
        [Column(name: "city", TypeName = "Varchar(50)")] public string City { get; set; } = string.Empty;
        [Column(name: "postal_code", TypeName = "Varchar(5)")] public string PostalCode { get; set; } = string.Empty;
        [Column(name: "address", TypeName = "Varchar(100)")] public string Address { get; set; } = string.Empty;
        [Column(name: "phone_number", TypeName = "Varchar(13)")] public string PhoneNumber { get; set; } = string.Empty;
        [Column(name: "user_credential_id")] public Guid UserCredentialId { get; set; }

        public virtual UserCredential? UserCredential { get; set; }
    }
}
