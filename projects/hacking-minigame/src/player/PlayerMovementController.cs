using System.Collections.Generic;
using Godot;
using network;
using shared;

namespace player;

public partial class PlayerMovementController : Node2D
{
    [Export] public Network Network;
    [Export] public Player Player;

    public NetworkNode PathPreviewTarget = null;
    public Queue<NetworkNode> PlayerMovementPath = new();
    public Tween PlayerMovementAnimation;

    public override void _Ready()
    {
        Network.NetworkNodeClicked += OnNetworkNodeClicked;
        Network.NetworkNodeMouseEnter += OnNetworkNodeMouseEnter;
        Network.NetworkNodeMouseExit += OnNetworkNodeMouseExit;

        // Move player to initial node
        if (Player.TargetMovementPosition is not null)
        {
            PlayerMovementPath.Enqueue(Player.TargetMovementPosition);
        }
    }

    public override void _Process(double delta)
    {
        if (PlayerMovementAnimation is null ||
        (PlayerMovementAnimation is not null && !PlayerMovementAnimation.IsRunning()))
        {
            // finished movement animation

            if (Player.TargetMovementPosition is not null)
            {
                if (Player.CurrentPosition is null || Player.TargetMovementPosition.GetInstanceId() != Player.CurrentPosition.GetInstanceId())
                {
                    Player.CurrentPosition = Player.TargetMovementPosition;
                }
            }

            if (PlayerMovementPath.Count == 0)
            {
                // Nothing to do
                Network.PlayerNavigationPathVisualizer.HideVisualizedPath();
            }
            else
            {
                // Queue up next animation
                Network.PlayerNavigationPathVisualizer.VisualizePath(Network, PlayerMovementPath, Network.GetEdgesOfNodePath(PlayerMovementPath), trackedStartNode: Player);

                var nextPlayerWaypoint = PlayerMovementPath.Dequeue();

                Player.TargetMovementPosition = nextPlayerWaypoint;

                var newMovementAnimation = CreateTween();

                Player.MoveToNode(nextPlayerWaypoint, Player.MovementSpeed, existingTween: newMovementAnimation);

                PlayerMovementAnimation = newMovementAnimation;

                if (PathPreviewTarget is not null)
                {
                    var (pathNodes, pathEdges) = Network.GetNavigationPath(start: Player.TargetMovementPosition, end: PathPreviewTarget);

                    Network.PlayerNavigationPreviewVisualizer.VisualizePath(Network, pathNodes, pathEdges, trackedStartNode: Player);
                }
            }
        }
    }

    private void OnNetworkNodeClicked(NetworkNode node, Network network)
    {
        var (pathNodes, _) = network.GetNavigationPath(start: Player.TargetMovementPosition, end: node);

        PlayerMovementPath = new(pathNodes);

        Network.PlayerNavigationPathVisualizer.VisualizePath(Network, PlayerMovementPath, Network.GetEdgesOfNodePath(PlayerMovementPath), trackedStartNode: Player);
    }

    void OnNetworkNodeMouseEnter(NetworkNode node, Network network)
    {
        PathPreviewTarget = node;

        var (pathNodes, pathEdges) = Network.GetNavigationPath(start: Player.TargetMovementPosition, end: PathPreviewTarget);

        Network.PlayerNavigationPreviewVisualizer.VisualizePath(Network, pathNodes, pathEdges, trackedStartNode: Player);
    }

    void OnNetworkNodeMouseExit(NetworkNode _, Network network)
    {
        PathPreviewTarget = null;
        network.PlayerNavigationPreviewVisualizer.HideVisualizedPath();
    }
}
