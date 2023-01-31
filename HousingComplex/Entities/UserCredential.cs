using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingComplex.Entities
{
    [Table("m_user_credential")]
    public class UserCredential
    {
        [Key,Column(name: "id")] public Guid Id { get; set; }
        [Column(name: "email"), EmailAddress, Required] public string Email { get; set; } = null!;
        [Column(name: "password"), Required, StringLength(maximumLength: int.MaxValue, MinimumLength = 6)] public string Password { get; set; } = null!;
        [Column(name: "role_id")] public Guid RoleId { get; set; }

        public virtual Role? Role { get; set; }
    }
}
