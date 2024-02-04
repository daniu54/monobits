using Godot;
using Godot.Collections;
using network;
using shared;
using static Godot.GD;

namespace player;

public partial class Player : Node2D
{
    [Export] public Network network;
    [Export] public NetworkNode networkNode;
    [Export] Array<Node2D> nodePath;

    public override async void _Ready()
    {
        var movementAnimation = CreateTween();

        foreach (var node in nodePath)
        {
            // TODO make movementSpeed configurable
            this.MoveToNode(node, movementSpeed: 200, existingTween: movementAnimation);
        }

        await movementAnimation.GetFinishedAwaiter();
    }
}
