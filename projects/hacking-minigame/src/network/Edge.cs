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
            _from = value;
            _from.NetworkNodePositionChanged += OnNetworkNodePositionChanged;
            Redraw();
        }
    }

    [Export]
    public NetworkNode To
    {
        get => _to;
        set
        {
            _to = value;
            _to.NetworkNodePositionChanged += OnNetworkNodePositionChanged;
            Redraw();
        }
    }

    private NetworkNode _from;
    private NetworkNode _to;

    public override void _Ready()
    {
        SubsctibeToNetworkNodeEvents();
        PropertyListChanged += Redraw;
        Redraw();
    }

    public override void _Draw()
    {
        if (From is not null && To is not null)
        {
            DrawLine(From.Position, To.Position, Colors.Gray, 4f);
        }
    }

    public override void _Notification(int notification)
    {
        if (notification == NotificationDragEnd)
        {
            return;
        }
    }

    private void SubsctibeToNetworkNodeEvents()
    {
        if (From is not null)
        {
            From.NetworkNodePositionChanged += OnNetworkNodePositionChanged;
        }

        if (To is not null)
        {
            To.NetworkNodePositionChanged += OnNetworkNodePositionChanged;
        }
    }

    private void OnNetworkNodePositionChanged(NetworkNode node)
    {
        Redraw();
    }

    public void Initialize(Network newNetwork)
    {
        network = newNetwork;
    }

    public void Redraw()
    {
        QueueRedraw();
    }
}
