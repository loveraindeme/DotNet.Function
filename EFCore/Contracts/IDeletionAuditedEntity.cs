namespace EFCore.Contracts
{
    public interface IDeletionAuditedEntity : IDeletionAuditedEntity<Guid?>
    {

    }

    public interface IDeletionAuditedEntity<TKey>
    {
        TKey DeleterId { get; set; }

        DateTime? DeletionTime { get; set; }

        bool IsDeleted { get; set; }
    }
}
