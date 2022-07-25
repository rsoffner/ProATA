namespace ProATA.SharedKernel.Interfaces
{
    public interface ICommand
    {
        void Execute();

        void Undo();
    }
}
