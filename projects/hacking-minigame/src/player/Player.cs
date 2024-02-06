using Godot;
using Godot.Collections;
using network;

namespace player;

public partial class Player : Node2D
{
    [Export] public Network network;
    [Export] public NetworkNode networkNode;
    [Export] Array<NetworkNode> nodePath;

    [Export] public int MovementSpeed = 200;
}
