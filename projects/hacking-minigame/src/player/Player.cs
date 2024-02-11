using Godot;
using network;

namespace player;

public partial class Player : Node2D
{
    [Export] public Network network;
    [Export] public NetworkNode networkNode;

    [Export] public int MovementSpeed = 200;
}
