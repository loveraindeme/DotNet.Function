namespace SugarSqlCore.Abstraction.Entities
{
    public interface ICreationAuditedEntity
    {
        Guid? CreatorId { get; set; }

        DateTime CreationTime { get; set; }
    }

    public interface ICreationAuditedEntity<TKey>
    {
        TKey? CreatorId { get; set; }

        DateTime CreationTime { get; set; }
    }
}
