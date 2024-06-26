using Godot;
using System;
using System.Globalization;

public class CropSlider : MarginContainer
{
    [Signal]
    public delegate void SliderValueChanged( float oldVal, float newVal );

    private RichTextLabel PepperLabel;
    private RichTextLabel TomatoLabel;
    public Slider Slider { get; private set; }

    private float CurrentSliderValue;
    private int PepperMax;
    private int TomatoMax;

    public override void _Ready()
    {
        PepperLabel = GetNode<RichTextLabel>("VBoxContainer/HBoxContainer2/PepperLabel");
        TomatoLabel = GetNode<RichTextLabel>("VBoxContainer/HBoxContainer2/TomatoLabel");
        Slider = GetNode<Slider>("VBoxContainer/HBoxContainer/HSlider");
        PepperMax = 1;
        TomatoMax = 1;

        Modulate = new Color(1,1,1,0);
    }

    public async void Initialize( int firstCrop, int secondCrop )
    {
        PepperMax = firstCrop;
        TomatoMax = secondCrop;
        UpdateLabels();

        Slider.Value = 0.5f;

        Tween tween = new Tween();
        tween.InterpolateProperty(this, "rect_position:y", -30, 0, 1.5f, Tween.TransitionType.Quad, Tween.EaseType.Out );
        AddChild(tween);
        tween.Start();
        
    
        Tween tween2 = new Tween();
        tween2.InterpolateProperty(this, "modulate", new Color(1,1,1,0), new Color(1,1,1,1), 1, Tween.TransitionType.Quad, Tween.EaseType.Out );
        AddChild(tween2);
        tween2.Start();

        await ToSignal( tween, "tween_completed" );
        tween.QueueFree();
        tween2.QueueFree();
    }

    public async void TweenValueTo( int newVal, bool instant = false )
	{
		// 26 Dec 2023 - Tween.TransitionType has the 'Trans' part removed.
		// So instead of Tween.TransitionType.TransLinear, it would be
		// Tween.TransitionType.Linear
		// The same applies to EaseType, where the 'Ease' part is removed.
		Tween tween = new Tween();
		tween.InterpolateProperty(Slider, "value", Slider.Value, newVal, 1 - Convert.ToInt32(instant)*1, Tween.TransitionType.Quad, Tween.EaseType.Out);
		AddChild(tween);
		tween.Start();
        await ToSignal( tween, "tween_completed" );
        tween.QueueFree();
	}

    private void UpdateLabels()
    {
        var pepper = CurrentSliderValue * PepperMax;
        pepper = (float)Math.Round( pepper, 2 );
        PepperLabel.BbcodeText = $"[center]Peppers[/center]\n[center]{pepper}[/center]";
        
        var tomato = (1 - CurrentSliderValue) * TomatoMax;
        tomato = (float)Math.Round( tomato, 2 );
        TomatoLabel.BbcodeText = $"[center]Tomatoes[/center]\n[center]{tomato}[/center]";
    }

    private void _OnSliderUpdate( float value )
    {
        if ( value != CurrentSliderValue )
        {
            EmitSignal( nameof(SliderValueChanged), CurrentSliderValue, value );
            CurrentSliderValue = value;
            UpdateLabels();
        }
    }
}
