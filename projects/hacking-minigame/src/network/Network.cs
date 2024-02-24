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

    private AStar2D navigation;

    [Signal] public delegate void NetworkNodeClickedEventHandler(NetworkNode node, Network network);
    [Signal] public delegate void NetworkNodeMouseEnterEventHandler(NetworkNode node, Network network);
    [Signal] public delegate void NetworkNodeMouseExitEventHandler(NetworkNode node, Network network);

    public override void _Ready()
    {
        navigation = new();

        Nodes = new Array<NetworkNode>(GetNode("NetworkNodes").GetChildren<NetworkNode>());
        Edges = new Array<Edge>(GetNode("Edges").GetChildren<Edge>());

        foreach (var node in Nodes)
        {
            node.NetworkNodeClicked += OnNetworkNodeClicked;
            node.NetworkNodeMouseEnter += OnNetworkNodeMouseEnter;
            node.NetworkNodeMouseExit += OnNetworkNodeMouseExit;

            navigation.AddPoint((long)node.GetInstanceId(), node.GlobalPosition);
        }

        foreach (var edge in Edges)
        {
            navigation.ConnectPoints((long)edge.From.GetInstanceId(), (long)edge.To.GetInstanceId());
        }
    }

    public (List<NetworkNode>, List<Edge>) GetNavigationPath(NetworkNode start, NetworkNode end)
    {
        var resultNodes = new List<NetworkNode>();

        var pathNodeIds = navigation.GetIdPath((long)start.GetInstanceId(), (long)end.GetInstanceId()).ToList();

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
