using System;
using System.Linq;
using Godot;
using network;

public partial class Level : Node2D
{
    [Export] public Network Network;
    [Export] public NetworkNode PlayerStartingPosition;

    public override void _Ready()
    {
        Network ??= GetNode<Network>("Network");
        PlayerStartingPosition = Network.Nodes.FirstOrDefault();

        ArgumentNullException.ThrowIfNull(PlayerStartingPosition);
    }
}
