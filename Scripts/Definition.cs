using Godot;
using System;

public class Definition : Button
{
    public async void Initialize()
    {
        Modulate = new Color(1,1,1,0);

        Tween tween = new Tween();
        tween.InterpolateProperty(this, "rect_position:y", RectPosition.y+30, RectPosition.y, 1f, Tween.TransitionType.Quad, Tween.EaseType.Out );
        AddChild(tween);
        tween.Start();
        
    
        Tween tween2 = new Tween();
        tween2.InterpolateProperty(this, "modulate", new Color(1,1,1,0), new Color(1,1,1,1), 0.75f, Tween.TransitionType.Quad, Tween.EaseType.Out );
        AddChild(tween2);
        tween2.Start();

        await ToSignal( tween, "tween_completed" );
        tween.QueueFree();
        tween2.QueueFree();
    }
}
