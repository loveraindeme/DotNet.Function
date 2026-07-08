using EFCore.Contracts;

namespace EFCore
{
    public class CreationAuditedEntity : CreationAuditedEntity<Guid?>, ICreationAuditedEntity<Guid?>
    {

    }

    public class CreationAuditedEntity<TKey> : ICreationAuditedEntity<TKey>
    {
        public TKey CreatorId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
