using Godot;
using System;

public class FarmCrops : Node2D
{
    private Node2D PepperContainer;
    private Node2D TomatoContainer;
    private Node2D PepperOccluder;
    private Node2D TomatoOccluder;
    
    private Vector2 PepperOccluderStart;
    private Vector2 PepperOccluderEnd;
    private Vector2 TomatoOccluderStart;
    private Vector2 TomatoOccluderEnd;

    private Vector2 CropIncrement;
    private Vector2 SecondRowOffset;

    private PackedScene _Crop;

    public override void _Ready()
    {
        PepperContainer = GetNode<Node2D>("Pepper");
        TomatoContainer = GetNode<Node2D>("Tomato");
        PepperOccluder = GetNode<Node2D>("Pepper/Light2D");
        TomatoOccluder = GetNode<Node2D>("Tomato/Light2D");

        PepperOccluderStart = new Vector2( 144, 0 );
        PepperOccluderEnd = new Vector2( 64, 0 );
        TomatoOccluderStart = new Vector2( -80, 0 );
        TomatoOccluderEnd = new Vector2( 0, 0 );
        
        CropIncrement = new Vector2( 20, 0 );
        SecondRowOffset = new Vector2( 10, 20 );

        _Crop = GD.Load<PackedScene>("res://Scenes/Crop.tscn");
    }

    public async void PlantCrops()
    {
        Timer timer = new Timer();
        timer.WaitTime = 0.05f;
        timer.Autostart = true;
        AddChild( timer );

        for ( int i = 0; i < 3; i++ )
        {
            for ( int o = 0; o < 4; o++ )
            {
                var pepper = _Crop.Instance() as AnimatedSprite;
                PepperContainer.AddChild( pepper );
                pepper.Position = CropIncrement * o + new Vector2(0, i*40);
                pepper.LightMask = 2;
                pepper.Play();

                var tomato = _Crop.Instance() as AnimatedSprite;
                TomatoContainer.AddChild( tomato );
                tomato.Position = CropIncrement * o + new Vector2(0, i*40);
                tomato.Animation = "tomato";
                tomato.LightMask = 4;
                tomato.Play();

                await ToSignal( timer, "timeout" );
            }
            for ( int o = 0; o < 3; o++ )
            {
                var pepper = _Crop.Instance() as AnimatedSprite;
                PepperContainer.AddChild( pepper );
                pepper.Position = CropIncrement * o + SecondRowOffset + new Vector2(0, i*40);
                pepper.LightMask = 2;
                pepper.Play();

                var tomato = _Crop.Instance() as AnimatedSprite;
                TomatoContainer.AddChild( tomato );
                tomato.Position = CropIncrement * o + SecondRowOffset + new Vector2(0, i*40);
                tomato.Animation = "tomato";
                tomato.LightMask = 4;
                tomato.Play();
            }
        }
    }

    public void UpdateCrops(float percent)
    {
        PepperOccluder.Position = PepperOccluderStart.LinearInterpolate( PepperOccluderEnd, 1-percent );
        TomatoOccluder.Position = TomatoOccluderStart.LinearInterpolate( TomatoOccluderEnd, percent );
    }
}
