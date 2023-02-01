using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingComplex.Entities
{
    [Table(name: "m_image_house_type")]
    public class ImageHouseType
    {
        [Key,Column(name: "id")] public Guid Id { get; set; }
        [Column(name: "filename")] public string FileName { get; set; } = string.Empty;
        [Column(name: "filesize")] public long FileSize { get; set; }
        [Column(name: "filepath")] public string FilePath { get; set; } = string.Empty;
        [Column(name: "content_type")] public string ContentType { get; set; } = string.Empty;
    }
}
