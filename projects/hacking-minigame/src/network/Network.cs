using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using shared;

namespace network;

public partial class Network : Node2D
{
    public Array<NetworkNode> Nodes = null;
    public Array<Edge> Edges = null;

    public NetworkNodePathVisualizer PlayerNavigationPathVisualizer;
    public NetworkNodePathVisualizer PlayerNavigationPreviewVisualizer;

    private AStar2D navigation;

    [Signal] public delegate void NetworkNodeClickedEventHandler(NetworkNode node, Network network);
    [Signal] public delegate void NetworkNodeMouseEnterEventHandler(NetworkNode node, Network network);
    [Signal] public delegate void NetworkNodeMouseExitEventHandler(NetworkNode node, Network network);

    public override void _Ready()
    {
        PlayerNavigationPathVisualizer = GetNode<NetworkNodePathVisualizer>("PlayerNavigationPathVisualizer");

        PlayerNavigationPreviewVisualizer = GetNode<NetworkNodePathVisualizer>("PlayerNavigationPreviewVisualizer");

        navigation = new();

        Nodes = GetNode("NetworkNodes").GetChildren<NetworkNode>();
        Edges = GetNode("Edges").GetChildren<Edge>();

        foreach (var node in Nodes)
        {
            node.Initialize(this);
            node.NetworkNodeClicked += OnNetworkNodeClicked;
            node.NetworkNodeMouseEnter += OnNetworkNodeMouseEnter;
            node.NetworkNodeMouseExit += OnNetworkNodeMouseExit;

            if (node.CanBeNavigatedOver)
            {
                navigation.AddPoint((long)node.GetInstanceId(), node.GlobalPosition);
            }
        }

        foreach (var edge in Edges)
        {
            edge.Initialize(this);

            if (edge.From.CanBeNavigatedOver && edge.To.CanBeNavigatedOver)
            {
                navigation.ConnectPoints((long)edge.From.GetInstanceId(), (long)edge.To.GetInstanceId());
            }
        }
    }

    public (List<NetworkNode>, List<Edge>) GetNavigationPath(NetworkNode start, NetworkNode end)
    {
        var resultNodes = new List<NetworkNode>();

        var pathNodeIds = new List<long>();

        var startId = (long)start.GetInstanceId();
        var endId = (long)end.GetInstanceId();

        if (navigation.GetPointIds().Contains(startId) && navigation.GetPointIds().Contains(endId))
        {
            pathNodeIds = navigation.GetIdPath(startId, endId).ToList();
        }

        // NOTE: need to preserve order of nodes in here
        foreach (var pathNodeId in pathNodeIds)
        {
            foreach (var node in Nodes)
            {
                if ((long)node.GetInstanceId() == pathNodeId)
                {
                    resultNodes.Add(node);
                }
            }
        }

        var resultEdges = GetEdgesOfNodePath(resultNodes);

        return (resultNodes, resultEdges);
    }

    public List<Edge> GetEdgesOfNodePath(IEnumerable<NetworkNode> nodePath)
    {
        var resultEdges = new List<Edge>();

        foreach (var node in nodePath)
        {
            foreach (var edge in Edges)
            {
                if (nodePath.Any(n => n.GetInstanceId() == edge.From.GetInstanceId()) &&
                nodePath.Any(n => n.GetInstanceId() == edge.To.GetInstanceId()))
                {
                    resultEdges.Add(edge);
                }
            }
        }

        return resultEdges;
    }

    private void OnNetworkNodeClicked(NetworkNode node)
    {
        EmitSignal(nameof(NetworkNodeClicked), node, this);
    }

    private void OnNetworkNodeMouseEnter(NetworkNode node)
    {
        EmitSignal(nameof(NetworkNodeMouseEnter), node, this);
    }

    private void OnNetworkNodeMouseExit(NetworkNode node)
    {
        EmitSignal(nameof(NetworkNodeMouseExit), node, this);
    }
}
