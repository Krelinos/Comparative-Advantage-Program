using Godot;
using System;

public class Scenario1bInputVisuals : Node2D
{

    private AnimatedSprite Actor1;
    private AnimatedSprite Actor2;
    private Sprite PumpkinPie;
    private Sprite Cheesecake;

    public override void _Ready()
    {
        Actor1 = GetNode<AnimatedSprite>("Actor1");
        Actor2 = GetNode<AnimatedSprite>("Actor2");
        PumpkinPie = GetNode<Sprite>("PumpkinPie");
        Cheesecake = GetNode<Sprite>("Cheesecake");
    }
    
    public async void OnDialogVisualsEvent( String visualsId )
    {
        switch ( visualsId )
        {
            case "show_food":
                PumpkinPie.Visible = true;
                Cheesecake.Visible = true;
                Actor1.Animation = "hands_up";
                Actor2.Animation = "hands_up";
                break;
            case "show_minutes_table":
                PumpkinPie.Visible = false;
                Cheesecake.Visible = false;
                Actor1.Animation = "idle_down";
                Actor2.Animation = "idle_down";
                break;
            case "move_actors_to_counters":
                // I don't really like how I set up this sequence.
                // I'm sure I can make something more scalable,
                // but this should be fine for the complexity of
                // this application.
                var tween = new Tween();
                tween.InterpolateProperty( Actor1, "position:x", 4, -8, 0.75f );
                AddChild( tween );
                tween.Start();
                Actor1.Playing = true;
                Actor1.Animation = "walk_side";
                Actor1.FlipH = true;

                await ToSignal( tween, "tween_completed" );

                var tween2 = new Tween();
                tween2.InterpolateProperty( Actor1, "position:y", -24, -18, 0.5f );
                AddChild( tween2 );
                tween2.Start();
                
                Actor1.Animation = "walk_down";
                Actor1.FlipH = false;
                
                await ToSignal( tween2, "tween_completed" );

                Actor1.Animation = "idle_down";
                Actor1.Playing = false;

                var tween3 = new Tween();
                tween3.InterpolateProperty( Actor2, "position:y", -24, -18, 0.5f );
                AddChild( tween3 );
                tween3.Start();
                Actor2.Playing = true;
                Actor2.Animation = "walk_down";

                await ToSignal( tween3, "tween_completed" );

                var tween4 = new Tween();
                tween4.InterpolateProperty( Actor2, "position:x", 28, 12, 0.75f );
                AddChild( tween4 );
                tween4.Start();
                Actor2.Playing = true;
                Actor2.Animation = "walk_side";
                Actor2.FlipH = true;

                await ToSignal( tween4, "tween_completed" );
                Actor2.Playing = false;
                Actor2.Animation = "idle_side";
                break;
            case "baker_create_pumpkin":
                Actor1.Animation = "hands_up";
                PumpkinPie.Visible = true;
                PumpkinPie.Position = new Vector2( -8, -30 );

                GetNode<Control>("SpeechBubble").Visible = true;
                break;
            case "show_opcost_table":
                Actor1.Animation = "idle_down";
                PumpkinPie.Position = new Vector2( -8, -6 );
                GetNode<Control>("SpeechBubble").Visible = false;
                break;
        }
    }
}
