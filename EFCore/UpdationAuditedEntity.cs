using EFCore.Contracts;

namespace EFCore
{
    public class UpdationAuditedEntity : UpdationAuditedEntity<Guid?>, IUpdationAuditedEntity
    {

    }

    public class UpdationAuditedEntity<TKey> : IUpdationAuditedEntity<TKey>
    {
        public TKey LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}
