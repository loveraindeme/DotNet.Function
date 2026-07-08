namespace EFCore.Contracts
{
    public interface IUpdationAuditedEntity : IUpdationAuditedEntity<Guid?>
    {

    }

    public interface IUpdationAuditedEntity<TKey>
    {
        TKey LastModifierId { get; set; }

        DateTime? LastModificationTime { get; set; }
    }
}
