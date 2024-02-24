using Godot;
using network;

public partial class Level : Node2D
{
    [Export] public Network Network;

    public override void _Ready()
    {
        Network ??= GetNode<Network>("Network");
    }
}
