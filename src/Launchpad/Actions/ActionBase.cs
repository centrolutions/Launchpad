using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace Launchpad.Actions
{
    public abstract class ActionBase : ObservableObject, IAction, ICloneable
    {
        public string Name { get; protected set; }

        public object Clone()
        {
            var clone = (ActionBase)this.MemberwiseClone();
            return clone;
        }

        public abstract void Execute();
        public abstract void ClearSettings();
    }
}
