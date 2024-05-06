using Godot;
using System;

public class FarmCrops : Node2D
{
    private Node2D PepperContainer;
    private Node2D TomatoContainer;
    private Light2D PepperOccluder;
    private Light2D TomatoOccluder;
    
    private Vector2 PepperOccluderStart;
    private Vector2 PepperOccluderEnd;
    private Vector2 TomatoOccluderStart;
    private Vector2 TomatoOccluderEnd;

    private Vector2 CropIncrement;
    private Vector2 SecondRowOffset;
    private int XRepeat;
    private int YRepeat;
    private int PepperMask;
    private int TomatoMask;

    private PackedScene _Crop;

    public override void _Ready()
    {
        PepperContainer = GetNode<Node2D>("Pepper");
        TomatoContainer = GetNode<Node2D>("Tomato");
        PepperOccluder = GetNode<Light2D>("Pepper/Light2D");
        TomatoOccluder = GetNode<Light2D>("Tomato/Light2D");

        PepperOccluderStart = new Vector2( 144, 0 );
        PepperOccluderEnd = new Vector2( 64, 0 );
        TomatoOccluderStart = new Vector2( -80, 0 );
        TomatoOccluderEnd = new Vector2( 0, 0 );

        _Crop = GD.Load<PackedScene>("res://Scenes/Crop.tscn");
    }

    public void Initialize( Vector2 cropIncre, Vector2 secondOff, int xRep, int yRep, int pepperMask, int tomatoMask )
    {
        CropIncrement = cropIncre;
        SecondRowOffset = secondOff;
        XRepeat = xRep;
        YRepeat = yRep;
        PepperMask = pepperMask;
        TomatoMask = tomatoMask;

        PepperOccluder.RangeItemCullMask = pepperMask;
        TomatoOccluder.RangeItemCullMask = tomatoMask;
    }

    public async void PlantCrops( )
    {
        Timer timer = new Timer();
        timer.WaitTime = 0.05f;
        timer.Autostart = true;
        AddChild( timer );

        for ( int i = 0; i < YRepeat; i++ )
        {
            for ( int o = 0; o < XRepeat; o++ )
            {
                var pepper = _Crop.Instance() as AnimatedSprite;
                PepperContainer.AddChild( pepper );
                pepper.Position = CropIncrement * o + new Vector2(0, i*40);
                pepper.LightMask = PepperMask;
                pepper.Play();

                var tomato = _Crop.Instance() as AnimatedSprite;
                TomatoContainer.AddChild( tomato );
                tomato.Position = CropIncrement * o + new Vector2(0, i*40);
                tomato.Animation = "tomato";
                tomato.LightMask = TomatoMask;
                tomato.Play();

                await ToSignal( timer, "timeout" );
            }
            for ( int o = 0; o < XRepeat-1; o++ )
            {
                var pepper = _Crop.Instance() as AnimatedSprite;
                PepperContainer.AddChild( pepper );
                pepper.Position = CropIncrement * o + SecondRowOffset + new Vector2(0, i*40);
                pepper.LightMask = PepperMask;
                pepper.Play();

                var tomato = _Crop.Instance() as AnimatedSprite;
                TomatoContainer.AddChild( tomato );
                tomato.Position = CropIncrement * o + SecondRowOffset + new Vector2(0, i*40);
                tomato.Animation = "tomato";
                tomato.LightMask = TomatoMask;
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
