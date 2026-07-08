namespace EFCore.Contracts
{
    public interface ICreationAuditedEntity : ICreationAuditedEntity<Guid?>
    {

    }

    public interface ICreationAuditedEntity<TKey>
    {
        TKey CreatorId { get; set; }

        DateTime CreationTime { get; set; }
    }
}
