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
        Connect("toggled", this, nameof(OnButtonToggled));
    }

    protected void OnButtonDown()
    {
        if ( !Disabled )
            Modulate = new Color(0.8f, 0.8f, 0.8f);
    }
    protected void OnButtonUp()
    {
        if ( !Disabled )
            if ( Pressed )
                Modulate = new Color( 1, 1, 1 );
            else
                Modulate = new Color(0.9f, 0.9f, 0.9f);

    }
    protected void OnMouseEntered()
    {
        if ( !Disabled && !Pressed )
            Modulate = new Color(1f, 1f, 1f);
    }
    protected void OnMouseExited()
    {
        if ( !Disabled && !Pressed )
            Modulate = new Color(0.9f, 0.9f, 0.9f);
    }
    protected virtual void OnButtonToggled( bool isPressed )
    {
        GD.Print( Name + ": " + isPressed );
        if ( isPressed )
            Modulate = new Color( 1, 1, 1 );
        else
            Modulate = new Color(0.9f, 0.9f, 0.9f);
    }
}
