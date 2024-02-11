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
    [Export] Array<NetworkNode> nodePath;

    public override async void _Ready()
    {
        foreach (var node in nodePath)
        {
            // TODO make movementSpeed configurable
            await this.MoveToNode(node, movementSpeed: 200).GetFinishedAwaiter();
            networkNode = node;
        }
    }
}
