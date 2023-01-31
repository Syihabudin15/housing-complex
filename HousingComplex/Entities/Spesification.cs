using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingComplex.Entities
{
    [Table("m_spesification")]
    public class Spesification
    {
        [Key,Column(name: "id")] public Guid Id { get; set; }
        [Column(name: "bedrooms")] public int Bedrooms { get; set; }
        [Column(name: "bathrooms")] public int Bathrooms { get; set; }
        [Column(name: "kitchens")] public int Kitchens { get; set; }
        [Column(name: "carport")] public bool Carport { get; set; }
        [Column(name: "swimming_pool")] public bool SwimmingPool { get; set; }
        [Column(name: "second_floor")] public bool SecondFloor { get; set; }
    }
}
