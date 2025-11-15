namespace SecurityService.Domain.Events;

public record UserUpdatedEvent
{
    public Guid Id { get; set; }
    public Dictionary<string, object> Changes { get; set; }
}