using Godot;
using network;
using shared;
using System;
using System.Linq;

public partial class LevelLoader : Node2D
{
    [Export] public PackedScene LevelScene;
    [Export] public Level Level;
    [Export] public Network Network;

    public override void _Ready()
    {
        Level ??= this.GetChildren<Level>().FirstOrDefault();

        if (Level is null)
        {
            ArgumentNullException.ThrowIfNull(LevelScene);

            Level = LevelScene.Instantiate<Level>();
            this.AddChild(Level);
        }

        ArgumentNullException.ThrowIfNull(Level);

        Network = Level.Network;

        ArgumentNullException.ThrowIfNull(Network);
    }
}
