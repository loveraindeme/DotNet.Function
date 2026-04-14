namespace SugarSqlCore.Abstraction.Entities
{
    public interface IDeletionAuditedEntity
    {
        Guid? DeleterId { get; set; }

        DateTime? DeletionTime { get; set; }

        bool IsDeleted { get; set; }
    }
}
