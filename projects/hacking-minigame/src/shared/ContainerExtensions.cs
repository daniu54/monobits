using System.Collections.Generic;
using System.Linq;
using Godot;

namespace shared;

public static class ControlExtensions
{
    public static void ResizeControlUsingContainerRatio(this Control control, float newRatio)
    {
        var ratioToCompensate = control.SizeFlagsStretchRatio - newRatio;

        control.SizeFlagsStretchRatio = newRatio;

        var containerSiblings = new List<Control>(control.GetParent().GetChildren<Control>())
            .Where(s => s.GetInstanceId() != control.GetInstanceId()).ToList();

        var ratioToCompensatePerSibling = ratioToCompensate / containerSiblings.Count;

        foreach (var parentSibling in containerSiblings)
        {
            parentSibling.SizeFlagsStretchRatio += ratioToCompensatePerSibling;
        }
    }
}
