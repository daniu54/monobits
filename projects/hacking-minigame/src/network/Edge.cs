using Godot;

namespace network;

[Tool]
public partial class Edge : Node2D
{
    [Export] public Network network;

    [Export]
    public NetworkNode From
    {
        get => _from;
        set
        {
            if (_from is not null)
            {
                _from.NetworkNodePositionChanged -= OnNetworkNodePositionChanged;
            }
            _from = value;
            if (_from is not null)
            {
                _from.NetworkNodePositionChanged += OnNetworkNodePositionChanged;
                RedrawEdge();
            }
        }
    }

    [Export]
    public NetworkNode To
    {
        get => _to;
        set
        {
            if (_to is not null)
            {
                _to.NetworkNodePositionChanged -= OnNetworkNodePositionChanged;
            }
            _to = value;
            if (_to is not null)
            {
                _to.NetworkNodePositionChanged += OnNetworkNodePositionChanged;
                RedrawEdge();
            }
        }
    }

    private NetworkNode _from;
    private NetworkNode _to;

    public override void _Draw()
    {
        if (From is not null && To is not null)
        {
            DrawLine(From.Position, To.Position, Colors.Gray, 4f);
        }
    }

    private void OnNetworkNodePositionChanged(NetworkNode node)
    {
        RedrawEdge();
    }

    public void Initialize(Network newNetwork)
    {
        network = newNetwork;
    }

    public void RedrawEdge()
    {
        if (From is not null && To is not null)
        {
            QueueRedraw();
        }
    }
}
