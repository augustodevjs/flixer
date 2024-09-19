using Flixer.Catalog.Domain.SeedWork;

namespace Flixer.Catalog.Domain.Events;

public class VideoUploadedEvent : DomainEvent
{
    public Guid ResourceId { get; set; }
    public string FilePath { get; set; }
    
    public VideoUploadedEvent(Guid resourceId, string filePath)
    {
        FilePath = filePath;
        ResourceId = resourceId;
    }
}