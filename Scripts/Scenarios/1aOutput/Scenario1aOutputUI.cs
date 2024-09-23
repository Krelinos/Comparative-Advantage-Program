using ComparativeAdvantage;
using Godot;
using System;

public class Scenario1aOutputUI : VBoxContainer, IScenarioVisualsOrUI
{
    [Signal]
    public delegate void LightCropChanged(float oldVal, float newVal);
    [Signal]
    public delegate void DarkCropChanged(float oldVal, float newVal);

    [Export] public NodePath _CropSliderContainer;

    private Control CropSliderContainer;
    private CropSlider LightCropSlider;
    private CropSlider DarkCropSlider;

    private Vector2 CropSliderContainerOriginalSize;

    public override void _Ready()
    {
        CropSliderContainer = GetNode<Control>( _CropSliderContainer );
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
        Initialize();
    }

    public void Initialize()
    {
        // GameService.Dialog.Connect("DialogVisualsEvent", this, nameof(_OnDialogVisualsEvent));
        LightCropSlider.TweenValueTo(0.5f, true);
        DarkCropSlider.TweenValueTo(0.5f, true);
    }

    public void OnDialogVisualsEvent( String visualsId )
    {
        switch ( visualsId )
        {
            case "plant_crops_light":
                LightCropSlider.Initialize( Main.Variables["light_pepper"], Main.Variables["light_tomato"] );
                break;
            case "plant_crops_dark":
                DarkCropSlider.Initialize( Main.Variables["dark_pepper"], Main.Variables["dark_tomato"] );
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
    //     CropSliderContainer.RectSize = CropSliderContainerOriginalSize/2;
    //     CropSliderContainer.RectScale = new Vector2(2, 2);
    }
}
