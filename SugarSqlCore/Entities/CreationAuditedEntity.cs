using SugarSqlCore.Abstraction.Entities;

namespace SugarSqlCore.Entities
{
    public class CreationAuditedEntity : ICreationAuditedEntity
    {
        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }
    }
}
