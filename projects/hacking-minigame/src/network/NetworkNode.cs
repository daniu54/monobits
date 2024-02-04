using Godot;

namespace network;

[Tool]
public partial class NetworkNode : Node2D
{
    [Export] public Area2D ClickableArea;
    [Export] public Network Network;

    [Signal] public delegate void NetworkNodePositionChangedEventHandler(NetworkNode node);

    public override void _Ready()
    {
        SetNotifyLocalTransform(true);

        ClickableArea = GetNode<Area2D>("ClickableArea");
    }

    public override void _Notification(int notification)
    {
        if (notification == NotificationLocalTransformChanged)
        {
            EmitSignal(nameof(NetworkNodePositionChanged), this);
            return;
        }
    }

    public void Initialize(Network newNetwork)
    {
        Network = newNetwork;
    }
}
