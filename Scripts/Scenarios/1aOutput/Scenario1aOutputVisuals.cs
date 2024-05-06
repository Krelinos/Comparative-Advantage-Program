using ComparativeAdvantage;
using Godot;
using System;

public class Scenario1aOutputVisuals : Node
{

    private Node2D LightPepperContainer;
    private Node2D LightTomatoContainer;
    private Node2D DarkPepperContainer;
    private Node2D DarkTomatoContainer;
    private Scenario1aOutputUI UI;

    private Vector2 LightIncrement;
    private Vector2 LightSecondRowOffset;
    private Vector2 DarkIncrement;
    private Vector2 DarkSecondRowOffset;

    private PackedScene _Crop;

    public override void _Ready()
    {
        LightPepperContainer = GetNode<Node2D>("LightCrops/Pepper");
        LightTomatoContainer = GetNode<Node2D>("LightCrops/Tomato");
        DarkPepperContainer = GetNode<Node2D>("DarkCrops/Pepper");
        DarkTomatoContainer = GetNode<Node2D>("DarkCrops/Tomato");

        LightIncrement = new Vector2( 20, 0 );
        LightSecondRowOffset = new Vector2( 10, 20 );

        _Crop = GD.Load<PackedScene>("res://Scenes/Crop.tscn");
    }

    public void Initialize()
    {
        UI = GameService.Scenario.CurrentScenarioUI as Scenario1aOutputUI;
        // UI.Connect( "LightCropChanged", this, nameof() );
        PlantLightCrops();
    }

    private async void PlantLightCrops()
    {
        Timer timer = new Timer();
        timer.WaitTime = 0.05f;
        timer.Autostart = true;
        AddChild( timer );
        GD.Print("beep");
        for ( int i = 0; i < 3; i++ )
        {
            for ( int o = 0; o < 4; o++ )
            {
                var pepper = _Crop.Instance() as AnimatedSprite;
                LightPepperContainer.AddChild( pepper );
                pepper.Position = LightIncrement * o + new Vector2(0, i*40);
                pepper.Play();

                var tomato = _Crop.Instance() as AnimatedSprite;
                LightTomatoContainer.AddChild( tomato );
                tomato.Position = LightIncrement * o + new Vector2(0, i*40);
                tomato.Animation = "tomato";
                tomato.Play();

                await ToSignal( timer, "timeout" );
            }
            for ( int o = 0; o < 3; o++ )
            {
                var pepper = _Crop.Instance() as AnimatedSprite;
                LightPepperContainer.AddChild( pepper );
                pepper.Position = LightIncrement * o + LightSecondRowOffset + new Vector2(0, i*40);
                pepper.Play();

                var tomato = _Crop.Instance() as AnimatedSprite;
                LightTomatoContainer.AddChild( tomato );
                tomato.Position = LightIncrement * o + LightSecondRowOffset + new Vector2(0, i*40);
                tomato.Animation = "tomato";
                tomato.Play();
            }
        }
    }
}
