using Godot;
using System;
using System.Globalization;

public class CropSlider : MarginContainer
{
    [Signal]
    public delegate void SliderValueChanged( float oldVal, float newVal );

    private RichTextLabel PepperLabel;
    private RichTextLabel TomatoLabel;
    private Slider Slider;

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
    }

    public void Initialize( int firstCrop, int secondCrop )
    {
        PepperMax = firstCrop;
        TomatoMax = secondCrop;
        UpdateLabels();
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
