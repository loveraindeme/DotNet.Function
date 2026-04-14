using SugarSqlCore.Abstraction.Entities;

namespace SugarSqlCore.Entities
{
    public class DeletionAuditedEntity : IDeletionAuditedEntity
    {
        public Guid? DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
