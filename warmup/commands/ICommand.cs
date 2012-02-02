namespace warmup.commands
{
    public interface ICommand
    {

        void Run(string[] args);
        void ShowHelp();

    }
}