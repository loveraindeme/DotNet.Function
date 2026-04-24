using SugarSqlCore.Abstraction.Entities;

namespace SugarSqlCore.Entities
{
    public class UpdationAuditedEntity : IUpdationAuditedEntity
    {
        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }

    public class UpdationAuditedEntity<TKey> : IUpdationAuditedEntity<TKey>
    {
        public TKey? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}
