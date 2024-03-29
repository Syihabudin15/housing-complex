﻿namespace HousingComplex.Dto.ImageType;

public class FileDownloadResponse
{
    public MemoryStream MemoryStream { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public string Filename { get; set; } = string.Empty;
}
