using System;
using System.Collections.Generic;
using System.Linq;

namespace Launchpad.Actions
{
    public class ActionFactory : IActionFactory
    {
        private IReadOnlyList<IAction> _Actions;
        public ActionFactory()
        {
            var actionType = typeof(IAction);
            _Actions = actionType.Assembly
                            .ExportedTypes
                            .Where(x => actionType.IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface)
                            .Select(x => Activator.CreateInstance(x))
                            .Cast<IAction>()
                            .OrderBy(x => x.Name)
                            .ToList();
        }
        public IReadOnlyList<string> GetAllNames()
        {
            return _Actions.Select(x => x.Name).ToList();
        }

        public IAction CreateNewByName(string name)
        {
            var actionType = _Actions.First(x => x.Name == name).GetType();

            return (IAction)Activator.CreateInstance(actionType);
        }
    }
}
