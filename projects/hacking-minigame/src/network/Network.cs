using Godot;
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

        foreach (var node in Nodes)
        {
            node.Initialize(this);
        }

        foreach (var edge in Edges)
        {
            edge.Initialize(this);
            edge.Redraw();
        }
    }
}
