namespace SugarSqlCore.Abstraction.Entities
{
    public interface ICreationAuditedEntity
    {
        Guid? CreatorId { get; set; }

        DateTime CreationTime { get; set; }
    }
}
