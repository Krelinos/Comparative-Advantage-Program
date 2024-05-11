using Godot;
using System;

public class MenuButton : Button
{
    public override void _Ready()
    {
        Connect("button_down", this, nameof(OnButtonDown));
        Connect("button_up", this, nameof(OnButtonUp));
        Connect("mouse_entered", this, nameof(OnMouseEntered));
        Connect("mouse_exited", this, nameof(OnMouseExited));
    }

    private void OnButtonDown()
    {
        if ( !Disabled )
            Modulate = new Color(0.8f, 0.8f, 0.8f);
    }
    private void OnButtonUp()
    {
        if ( !Disabled )
            Modulate = new Color(0.9f, 0.9f, 0.9f);
    }
    private void OnMouseEntered()
    {
        if ( !Disabled )
            Modulate = new Color(1f, 1f, 1f);
    }
    private void OnMouseExited()
    {
        if ( !Disabled )
            Modulate = new Color(0.9f, 0.9f, 0.9f);
    }
}
