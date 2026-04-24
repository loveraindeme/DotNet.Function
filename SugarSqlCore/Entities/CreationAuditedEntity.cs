using SugarSqlCore.Abstraction.Entities;

namespace SugarSqlCore.Entities
{
    public class CreationAuditedEntity : ICreationAuditedEntity
    {
        public Guid? CreatorId { get; set; }

        public DateTime CreationTime { get; set; }
    }

    public class CreationAuditedEntity<TKey> : ICreationAuditedEntity<TKey>
    {
        public TKey? CreatorId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
