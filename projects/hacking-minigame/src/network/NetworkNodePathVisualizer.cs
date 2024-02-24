using Godot;
using System.Collections.Generic;

namespace network;

public partial class NetworkNodePathVisualizer : Node2D
{
    [Export] Color PathNodeColor = Colors.Green;

    [Export] Color PathEdgeColor = Colors.Green;
    [Export] float PathEdgeLineWidt = 10f;

    [Export] Color TrackedStartNodeColor = Colors.Green;

    [Export] Color TrackedStartNodeEdgeColor = Colors.Green;
    [Export] float TrackedStartNodeEdgeLineWidt = 10f;

    public List<NetworkNode> MarkedNodes = new();
    public List<Edge> MarkedEdges = new();

    public Node2D TrackedStartNode = null;

    public override void _Process(double delta)
    {
        if (TrackedStartNode is not null)
        {
            QueueRedraw();
        }
    }

    public override void _Draw()
    {
        if (TrackedStartNode is not null && MarkedNodes.Count > 0)
        {
            DrawCircle(ToLocal(TrackedStartNode.GlobalPosition), radius: 64, TrackedStartNodeColor);
            DrawLine(ToLocal(TrackedStartNode.GlobalPosition), MarkedNodes[0].Position, TrackedStartNodeEdgeColor, TrackedStartNodeEdgeLineWidt);
        }

        foreach (var markedNode in MarkedNodes)
        {
            DrawCircle(markedNode.Position, radius: 64, PathNodeColor);
        }

        foreach (var markedEdge in MarkedEdges)
        {
            DrawLine(markedEdge.From.Position, markedEdge.To.Position, PathEdgeColor, PathEdgeLineWidt);
        }
    }

    public void VisualizePath(IEnumerable<NetworkNode> pathNodes, IEnumerable<Edge> pathEdges, Node2D trackedStartNode = null)
    {
        MarkedNodes = new(pathNodes);
        MarkedEdges = new(pathEdges);

        TrackedStartNode = trackedStartNode;

        this.Visible = true;
        QueueRedraw();
    }

    public void ShowVisualizedPath()
    {
        this.Visible = true;
    }

    public void HideVisualizedPath()
    {
        MarkedNodes.Clear();
        MarkedEdges.Clear();
        this.Visible = false;
    }
}
