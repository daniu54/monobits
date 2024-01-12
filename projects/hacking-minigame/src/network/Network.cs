using Godot;
using static Godot.GD;
using Godot.Collections;
using shared;

namespace network;

public partial class Network : Node2D
{
    [Export] public Array<NetworkNode> Nodes = null;
    [Export] public Array<Edge> Edges = null;

    public override void _Ready()
    {
        Nodes = GetNode("NetworkNodes").GetChildren<NetworkNode>();
        Edges = GetNode("Edges").GetChildren<Edge>();
    }

    public override void _Process(double delta)
    {
    }
}
