using Flixer.Catalog.Domain.Enums;
using Flixer.Catalog.Domain.SeedWork;

namespace Flixer.Catalog.Domain.Entities;

public class Media : Entity
{
    public string FilePath { get; private set; }
    public string? EncodedPath { get; private set; }
    public MediaStatus Status { get; private set; }

    public Media(string filePath)
    {
        FilePath = filePath;
        Status = MediaStatus.Pending;
    }
    
    public void UpdateAsSentToEncode()
        => Status = MediaStatus.Processing;
    
    public void UpdateAsEncoded(string encodedExamplePath)
    {
        Status = MediaStatus.Completed;
        EncodedPath = encodedExamplePath;
    }
    
    public void UpdateAsEncodingError()
    {
        Status = MediaStatus.Error;
        EncodedPath = null;
    }
}