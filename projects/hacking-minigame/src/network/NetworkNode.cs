using Godot;

namespace network;

[Tool]
public partial class NetworkNode : Node2D
{
    [Export] public Network network;

    [Signal] public delegate void NetworkNodePositionChangedEventHandler(NetworkNode node);

    public override void _Ready()
    {
        SetNotifyLocalTransform(true);
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
        network = newNetwork;
    }
}
