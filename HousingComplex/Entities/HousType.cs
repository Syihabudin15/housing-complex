﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingComplex.Entities
{
    [Table(name: "m_hous_type")]
    public class HousType
    {
        [Key, Column(name: "id")] public Guid Id { get; set; }
        [Column(name: "name", TypeName = "Varchar(100")] public string Name { get; } = string.Empty;
        [Column(name: "specification_id")] public Guid SpesificationId { get; set; }
        [Column(name: "description", TypeName = "Varchar(500)")] public string Description { get; set; } = string.Empty;
        [Column(name: "housing_id")] public Guid HousingId { get; set; }
        [Column(name: "price")] public long Price { get; set; }
        [Column(name: "image_id")] public Guid ImageId { get; set; }

        public virtual Spesification? Spesification { get; set; }
        public virtual Housing? Housing { get; set; }
        public virtual ImageHousType? ImageHousType { get; set; }

    }
}
