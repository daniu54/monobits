using System;
using System.Linq;
using Godot;
using network;
using player;

namespace ui;

public partial class NodeInformationSidebar : Control
{
    [Export] public Player Player;
    [Export] public Label CurrentNodeName;
    [Export] public ToggleCollapseButton SidebarToggle;

    public override void _Ready()
    {
        Player.ArrivedAtNode += OnPlayerArrivedAtNode;
    }

    private void OnPlayerArrivedAtNode(NetworkNode node, Player player)
    {
        CurrentNodeName.Text = node.Name;

        // display node name vertically
        string verticalString = string.Join('\n', node.Name.ToString().Select(c => c.ToString()));
        SidebarToggle.TextCollapsed = $">\n{verticalString}\n>";
    }
}
