using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingComplex.Entities
{
    [Table(name: "m_image_hous_type")]
    public class ImageHousType
    {
        [Key,Column(name: "id")] public Guid Id { get; set; }
        [Column(name: "filename")] public string FileName { get; set; } = string.Empty;
        [Column(name: "filesize")] public string FileSize { get; set; } = string.Empty;
        [Column(name: "filepath")] public string FilePath { get; set; } = string.Empty;
        [Column(name: "content_type")] public string ContentType { get; set; } = string.Empty;
    }
}
