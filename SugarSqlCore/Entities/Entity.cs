using SugarSqlCore.Abstraction.Entities;

namespace SugarSqlCore.Entities
{
    public abstract class Entity : IEntity
    {
        protected Entity()
        {

        }

        public abstract object? GetKeys();
    }

    public abstract class Entity<TKey> : Entity, IEntity<TKey>
    {
        public virtual TKey Id { get; protected set; } = default!;

        protected Entity()
        {

        }

        protected Entity(TKey id)
        {
            Id = id;
        }

        public override object? GetKeys()
        {
            return Id;
        }
    }
}
