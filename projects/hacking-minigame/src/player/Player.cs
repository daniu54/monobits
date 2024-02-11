using Godot;
using network;

namespace player;

public partial class Player : Node2D
{
    [Export] public Network Network;
    [Export] public NetworkNode NetworkNode;

    [Export] public int MovementSpeed = 200;
}
