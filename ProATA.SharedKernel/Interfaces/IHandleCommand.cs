namespace ProATA.SharedKernel.Interfaces
{
    public interface IHandleCommand<in T>
    {
        Task Handle(T command);
    }
}
