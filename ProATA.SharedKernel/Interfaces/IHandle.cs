namespace ProATA.SharedKernel.Interfaces
{
    public interface IHandle<T> where T : IDomainEvent
    {
        void Handle(T args);
    }
}
