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
        foreach (var node in this.nodePath)
        {
            // TODO make movementSpeed configurable
            await this.MoveToNode(node, movementSpeed: 200).GetFinishedAwaiter();
        }
    }
}
