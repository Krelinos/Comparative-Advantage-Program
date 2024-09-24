using Godot;
using System;

public class ScenarioMenuButton : MenuButton
{
    [Signal] public delegate void ScenarioButtonPressed( String scenName );

    public override void _Ready()
    {
        base._Ready();
        Connect( "pressed", this, nameof(OnMenuButtonPressed) );
    }
    
    private void OnMenuButtonPressed()
    {
        EmitSignal( nameof(ScenarioButtonPressed), Name );
    }
}
