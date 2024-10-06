using Godot;
using System;

public class TabMenuButton : MenuButton
{
    [Export] private NodePath _TabContainer;

    private Control TabContainer;

    public override void _Ready()
    {
        base._Ready();
        TabContainer = GetNode<Control>( _TabContainer );
    }

    protected override void OnButtonToggled( bool isPressed )
    {
        base.OnButtonToggled( isPressed );

        TabContainer.Visible = isPressed;
    }
}
