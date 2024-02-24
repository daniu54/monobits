using Godot;

namespace network;

[Tool]
public partial class NetworkNode : Node2D
{
    [Signal] public delegate void NetworkNodePositionChangedEventHandler(NetworkNode node);
    [Signal] public delegate void NetworkNodeClickedEventHandler(NetworkNode node);
    [Signal] public delegate void NetworkNodeMouseEnterEventHandler(NetworkNode node);
    [Signal] public delegate void NetworkNodeMouseExitEventHandler(NetworkNode node);

    private Area2D ClickableArea;

    public override void _Ready()
    {
        SetNotifyLocalTransform(true);

        ClickableArea = GetNode<Area2D>("ClickableArea");

        ClickableArea.InputPickable = true;
        ClickableArea.InputEvent += OnNodeClicked;
        ClickableArea.MouseEntered += OnNodeMouseEnter;
        ClickableArea.MouseExited += OnNodeMouseExit;
    }

    public override void _Notification(int notification)
    {
        if (notification == NotificationLocalTransformChanged)
        {
            EmitSignal(nameof(NetworkNodePositionChanged), this);
            return;
        }
    }

    private void OnNodeClicked(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.IsPressed() && mouseButton.ButtonIndex == MouseButton.Right)
            {
                EmitSignal(nameof(NetworkNodeClicked), this);
                return;
            }
        }
    }

    private void OnNodeMouseEnter()
    {
        EmitSignal(nameof(NetworkNodeMouseEnter), this);
    }

    private void OnNodeMouseExit()
    {
        EmitSignal(nameof(NetworkNodeMouseExit), this);
    }
}
