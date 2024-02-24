using Godot;
using network;

namespace player;

public partial class Player : Node2D
{
    [Signal] public delegate void ArrivedAtNodeEventHandler(NetworkNode node, Player player);

    [Export] public int MovementSpeed = 200;

    [Export] public NetworkNode TargetMovementPosition;

    public NetworkNode CurrentPosition
    {
        get => _currentPosition;
        set
        {
            _currentPosition = value;
            if (_currentPosition is not null)
            {
                EmitSignal(nameof(ArrivedAtNode), _currentPosition, this);
            }
        }
    }
    private NetworkNode _currentPosition;
}
