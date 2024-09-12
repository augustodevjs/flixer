using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Infra.Data.EF.Models;

public class VideosCastMembers
{
    public Guid VideoId { get; set; }
    public Guid CastMemberId { get; set; }
    
    public Video? Video { get; set; }
    public CastMember? CastMember { get; set; }

    public VideosCastMembers(Guid castMemberId, Guid videoId)
    {
        CastMemberId = castMemberId;
        VideoId = videoId;
    }
}