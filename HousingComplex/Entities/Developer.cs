using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingComplex.Entities
{
    [Table("m_developer")]
    public class Developer
    {
        [Key,Column(name: "id")] public Guid Id { get; set; }
        [Column(name: "name", TypeName = "Varchar(100)")] public string Name { get; set; } = string.Empty;
        [Column(name: "phone_number", TypeName = "Varchar(13)")] public string PhoneNumber { get; set; } = string.Empty;
        [Column(name: "user_credential_id")] public Guid UserCredentialId { get; set; }
        [Column(name: "address", TypeName = "Varchar(100)")] public string Address { get; set; } = string.Empty;

        public virtual UserCredential? UserCredential { get; set; }
        public virtual Housing? Housing { get; set; }
    }
}
