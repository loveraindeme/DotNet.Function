namespace EFCore.Contracts
{
    public interface IEntity
    {
        object? GetKey();
    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; }
    }
}
