using SugarSqlCore.Abstraction.Entities;

namespace SugarSqlCore.Entities
{
    public class UpdationAuditedEntity : IUpdationAuditedEntity
    {
        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}
