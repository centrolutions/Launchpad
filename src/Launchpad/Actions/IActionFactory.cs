using System.Collections.Generic;

namespace Launchpad.Actions
{
    public interface IActionFactory
    {
        IReadOnlyList<string> GetAllNames();
        IAction CreateNewByName(string name);
    }
}
