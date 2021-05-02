namespace Launchpad.Actions
{
    public interface IAction
    {
        string Name { get; }
        void Execute();
        void ClearSettings();
    }
}
