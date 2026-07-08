using EFCore.Contracts;

namespace EFCore
{
    public class DeletionAuditedEntity : DeletionAuditedEntity<Guid?>, IDeletionAuditedEntity<Guid?>
    {

    }

    public class DeletionAuditedEntity<TKey> : IDeletionAuditedEntity<TKey>
    {
        public TKey DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
