using ProATA.SharedKernel.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProATA.SharedKernel
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
    {
        [Key]
        public virtual TId Id { get; protected set; }

        [NotMapped]
        public virtual ICollection<IDomainEvent> Events { get; set; }

        protected Entity(TId id)
        {
            if (object.Equals(id, default(TId)))
            {
                throw new ArgumentException("The ID cannot be the type's default value.", "id");
            }

            this.Id = id;

            Events = new List<IDomainEvent>();
        }

        // EF requires an empty constructor
        protected Entity()
        {
        }

        public override bool Equals(object otherObject)
        {
            var entity = otherObject as Entity<TId>;
            if (entity != null)
            {
                return this.Equals(entity);
            }
            return base.Equals(otherObject);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public virtual bool Equals(Entity<TId> other)
        {
            if (other == null)
            {
                return false;
            }
            return this.Id.Equals(other.Id);
        }
    }
}
