using Godot;
using System;

namespace ComparativeAdvantage {

public class FarmCrops : Node2D
{
    private Node2D PepperContainer;
    private Node2D TomatoContainer;
    private Light2D PepperOccluder;
    private Light2D TomatoOccluder;
    private RectangleShape2D PlantingZone;
    
    private Vector2 PepperOccluderStart;
    private Vector2 PepperOccluderEnd;
    private Vector2 TomatoOccluderStart;
    private Vector2 TomatoOccluderEnd;

    private int XDensity;   // Plants will be XDensity apart in the X axis.
    private int YDensity;
    private int PepperMask;
    private int TomatoMask;

    private PackedScene _Crop;

    public override void _Ready()
    {
        PepperContainer = GetNode<Node2D>("Pepper");
        TomatoContainer = GetNode<Node2D>("Tomato");
        PepperOccluder = GetNode<Light2D>("Pepper/Occluder");
        TomatoOccluder = GetNode<Light2D>("Tomato/Occluder");
        PlantingZone = GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D;

        // PepperOccluder should start on the right then sweep left.
        PepperOccluderStart = new Vector2( PlantingZone.Extents.x, 0 )*2;
        PepperOccluderEnd = new Vector2( -PlantingZone.Extents.x, 0 )*2;
        // Opposite for the Tomato.
        TomatoOccluderStart = new Vector2( -PlantingZone.Extents.x, 0 )*2;
        TomatoOccluderEnd = new Vector2( PlantingZone.Extents.x, 0 )*2;

        _Crop = GD.Load<PackedScene>("res://Scenes/Crop.tscn");
    }

    public void Initialize( int _xDensity, int _yDensity, int pepperMask, int tomatoMask )
    {
        XDensity = _xDensity;
        YDensity = _yDensity;
        PepperMask = pepperMask;
        TomatoMask = tomatoMask;

        // Alter the pepper Light2Ds so that they adapt to the dynamic
        // nature of Area2D PlantingZone.
        PepperOccluder.RangeItemCullMask = PepperMask;
        PepperOccluder.Offset = PepperOccluderStart;

        var pepperOccluderTexture = PepperOccluder.Texture as GradientTexture2D;
        pepperOccluderTexture.Width = (int)PlantingZone.Extents.x*2;
        pepperOccluderTexture.Height = (int)PlantingZone.Extents.y*2;
        
        // Repeat for tomato Light2D.
        TomatoOccluder.RangeItemCullMask = TomatoMask;
        TomatoOccluder.Offset = TomatoOccluderStart;

        var tomatoOccluderTexture = TomatoOccluder.Texture as GradientTexture2D;
        tomatoOccluderTexture.Width = (int)PlantingZone.Extents.x*2;
        tomatoOccluderTexture.Height = (int)PlantingZone.Extents.y*2;
        PlantCrops();
    }

    /// <summary>
    /// Animates crops appearing based on Area2D PlantingZone. Adapts to the shape
    /// of PlantingZone.
    /// </summary>
    public async void PlantCrops( )
    {
        Timer timer = new Timer();
        timer.WaitTime = 0.05f;
        timer.Autostart = true;
        AddChild( timer );

        int xPlantingPos;
        int yPlantingPos = (int)-PlantingZone.Extents.y + YDensity;
        bool alternator = false;    // Used to create the "hexagon" planting pattern

        while ( yPlantingPos < PlantingZone.Extents.y )
        {
            xPlantingPos = (int)-PlantingZone.Extents.x + XDensity;
            while ( xPlantingPos < PlantingZone.Extents.x )
            {
                var pepper = _Crop.Instance() as AnimatedSprite;
                PepperContainer.AddChild( pepper );
                pepper.Position = new Vector2(
                    xPlantingPos + XDensity/2 * (alternator ? 1 : 0),
                    yPlantingPos
                );
                pepper.LightMask = PepperMask;
                pepper.Play();

                var tomato = _Crop.Instance() as AnimatedSprite;
                TomatoContainer.AddChild( tomato );
                tomato.Position = new Vector2(
                    xPlantingPos + XDensity/2 * (alternator ? 1 : 0),
                    yPlantingPos
                );
                tomato.Animation = "tomato";
                tomato.LightMask = TomatoMask;
                tomato.Play();

                xPlantingPos += XDensity;
                await ToSignal( timer, "timeout" );
            }
            alternator = !alternator;
            yPlantingPos += YDensity;
        }
    }

    public void UpdateCrops(float percent)
    {
        PepperOccluder.Position = PepperOccluderStart.LinearInterpolate( PepperOccluderEnd, 1-percent );
        TomatoOccluder.Position = TomatoOccluderStart.LinearInterpolate( TomatoOccluderEnd, percent );
    }
}

} // namespace ComparativeAdvantage