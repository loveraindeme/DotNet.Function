namespace SugarSqlCore.Abstraction.Entities
{
    public interface IUpdationAuditedEntity
    {
        Guid? LastModifierId { get; set; }

        DateTime? LastModificationTime { get; set; }
    }
}
