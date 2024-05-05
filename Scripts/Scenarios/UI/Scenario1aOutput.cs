using Godot;
using System;

public class Scenario1aOutput : VBoxContainer
{
    [Signal]
    public delegate void PepperCropChanged(float oldVal, float newVal);
    [Signal]
    public delegate void TomatoCropChanged(float oldVal, float newVal);

    private Control CropSliderContainer;
    private CropSlider PepperSlider;
    private CropSlider TomatoSlider;

    private Vector2 CropSliderContainerOriginalSize;

    public override void _Ready()
    {
        CropSliderContainer = GetNode<Control>("HBoxContainer");
        PepperSlider = CropSliderContainer.GetNode<CropSlider>("CropSlider");
        TomatoSlider = CropSliderContainer.GetNode<CropSlider>("CropSlider2");

        CropSliderContainerOriginalSize = RectSize;

        PepperSlider.Connect("SliderValueChanged", this, nameof(_PepperSliderChanged));
        TomatoSlider.Connect("SliderValueChanged", this, nameof(_TomatoSliderChanged));

        Timer timer = new Timer();
        AddChild( timer );
        timer.WaitTime = 0.1f;
        timer.Start();
        timer.Connect( "timeout", this, nameof(TimerTimeout));
        // await ToSignal(timer, "timeout");
        // timer.QueueFree();
    }

    public void _PepperSliderChanged(float oldVal, float newVal)
    {
        EmitSignal(nameof(PepperCropChanged), oldVal, newVal);
    }

    public void _TomatoSliderChanged(float oldVal, float newVal)
    {
        EmitSignal(nameof(TomatoCropChanged), oldVal, newVal);
    }

    public void TimerTimeout()
    {
        CropSliderContainer.RectSize = CropSliderContainerOriginalSize/2;
        CropSliderContainer.RectScale = new Vector2(2, 2);
    }
}
