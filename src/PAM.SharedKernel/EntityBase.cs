using System.ComponentModel.DataAnnotations.Schema;

namespace PAM.SharedKernel
{
  // This can be modified to EntityBase<TId> to support multiple key types (e.g. Guid)
  public abstract class EntityBase
  {
    public int Id { get; set; }

    public bool Active { get; set; } = true;

    private List<DomainEventBase> _domainEvents = new();
    [NotMapped]
    public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
    internal void ClearDomainEvents() => _domainEvents.Clear();
  }
}
