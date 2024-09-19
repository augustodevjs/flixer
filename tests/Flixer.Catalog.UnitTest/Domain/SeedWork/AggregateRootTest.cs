using Xunit;
using FluentAssertions;
using Flixer.Catalog.UnitTest.Domain.SeedWork.FakeClasses;

namespace Flixer.Catalog.UnitTest.Domain.SeedWork;

public class AggregateRootTest
{
    [Fact]
    [Trait("Domain", "AggregateRoot")]
    public void RaiseEvent()
    {
        var domainEvent = new DomainEventFake();
        var aggregate = new AggregateRootFake();
        
        aggregate.RaiseEvent(domainEvent);
        aggregate.Events.Should().HaveCount(1);
    }
    
    [Fact]
    [Trait("Domain", "AggregateRoot")]
    public void ClearEvents()
    {
        var domainEvent = new DomainEventFake();
        var aggregate = new AggregateRootFake();
        
        aggregate.RaiseEvent(domainEvent);
        
        aggregate.ClearEvents();
        aggregate.Events.Should().BeEmpty();
    }
}