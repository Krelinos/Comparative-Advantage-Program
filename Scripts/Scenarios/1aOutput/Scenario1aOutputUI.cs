using ComparativeAdvantage;
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

    public void Initialize()
    {
        GameService.Dialog.Connect("DialogVisualsEvent", this, nameof(_OnDialogVisualsEvent));
    }

    private void _OnDialogVisualsEvent( String visualsName )
    {
        switch ( visualsName )
        {
            case "plant_crops_light":
                LightCropSlider.Initialize( (int)GameService.Variables["light_pepper"], (int)GameService.Variables["light_tomato"] );
                break;
            case "plant_crops_dark":
                DarkCropSlider.Initialize( (int)GameService.Variables["dark_pepper"], (int)GameService.Variables["dark_tomato"] );
                break;
            case "light_only_pepper":
                LightCropSlider.TweenValueTo(1);
                break;
            case "light_only_tomato":
                LightCropSlider.TweenValueTo(0);
                break;
            case "dark_only_pepper":
                DarkCropSlider.TweenValueTo(1);
                break;
            case "dark_only_tomato":
                DarkCropSlider.TweenValueTo(0);
                break;
        }
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
