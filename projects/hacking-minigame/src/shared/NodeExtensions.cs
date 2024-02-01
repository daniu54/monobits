using Godot;
using Godot.Collections;

namespace shared;

public static class NodeExtensions
{
    /// <summary>
    /// Get all child nodes of <paramref name="node"/> of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The required type of child nodes.</typeparam>
    /// <param name="node">The <see cref="Node"/> to search.</param>
    /// <returns></returns>
    public static Array<T> GetChildren<[MustBeVariant] T>(this Node node)
        where T : Node
    {
        var result = new Array<T>();

        foreach (var child in node.GetChildren())
        {
            if (child is T)
            {
                result.Add(child as T);
            }
        }

        return result;
    }

    public static Tween MoveToNode(this Node2D node, Node2D target, int movementSpeed)
    {
        var distanceToNextNode = node.GlobalPosition.DistanceTo(target.GlobalPosition);

        var animationTime = distanceToNextNode / movementSpeed;

        var movementAnimation = node.CreateTween();

        movementAnimation.TweenProperty(node, "global_position", target.GlobalPosition, animationTime);

        return movementAnimation;
    }
}