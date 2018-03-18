using System;
using Unity.Build.Selected;

namespace Unity.Build.Pipeline
{

    public delegate SelectedConstructor SelectConstructor(Type type);

    public interface ISelectConstructor
    {
        SelectConstructor SelectConstructor { get; }
    }
}
