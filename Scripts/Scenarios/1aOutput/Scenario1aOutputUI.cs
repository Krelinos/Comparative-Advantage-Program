using Godot;
using System;

public class Scenario1aOutputUI : VBoxContainer
{
    [Signal]
    public delegate void LightCropChanged(float oldVal, float newVal);
    [Signal]
    public delegate void DarkCropChanged(float oldVal, float newVal);

    private Control CropSliderContainer;
    private CropSlider LightCropSlider;
    private CropSlider DarkCropSlider;

    private Vector2 CropSliderContainerOriginalSize;

    public override void _Ready()
    {
        CropSliderContainer = GetNode<Control>("HBoxContainer");
        LightCropSlider = CropSliderContainer.GetNode<CropSlider>("CropSlider");
        DarkCropSlider = CropSliderContainer.GetNode<CropSlider>("CropSlider2");

        CropSliderContainerOriginalSize = RectSize;

        LightCropSlider.Connect("SliderValueChanged", this, nameof(_LightSliderChanged));
        DarkCropSlider.Connect("SliderValueChanged", this, nameof(_DarkSliderChanged));

        Timer timer = new Timer();
        AddChild( timer );
        timer.WaitTime = 0.1f;
        timer.Start();
        timer.Connect( "timeout", this, nameof(TimerTimeout) );
        // await ToSignal(timer, "timeout");
        // timer.QueueFree();
    }

    public void _LightSliderChanged(float oldVal, float newVal)
    {
        EmitSignal(nameof(LightCropChanged), oldVal, newVal);
    }

    public void _DarkSliderChanged(float oldVal, float newVal)
    {
        EmitSignal(nameof(DarkCropChanged), oldVal, newVal);
    }

    public void TimerTimeout()
    {
        CropSliderContainer.RectSize = CropSliderContainerOriginalSize/2;
        CropSliderContainer.RectScale = new Vector2(2, 2);
    }
}
