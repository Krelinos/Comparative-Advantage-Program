using ComparativeAdvantage;
using Godot;
using System;

namespace ComparativeAdvantage {

public class Scenario1aOutputVisuals : Node
{
    private FarmCrops LightCrops;
    private FarmCrops DarkCrops;
    private Scenario1aOutputUI UI;

    public override void _Ready()
    {
        LightCrops = GetNode<FarmCrops>("LightCrops");
        DarkCrops = GetNode<FarmCrops>("DarkCrops");
        UI = Main.ScenarioUI as Scenario1aOutputUI;

        Initialize();
    }

    public void Initialize()
    {
        UI.Connect( nameof(Scenario1aOutputUI.LightCropChanged), this, nameof(UpdateLightCrops) );
        UI.Connect( nameof(Scenario1aOutputUI.DarkCropChanged), this, nameof(UpdateDarkCrops) );

        // GameService.Dialog.Connect("DialogVisualsEvent", this, nameof(_OnDialogVisualsEvent));
    
        LightCrops.Initialize( 20, 20, 2, 4 );
        LightCrops.UpdateCrops(0.5f);
        DarkCrops.Initialize( 15, 20, 8, 16 );
        DarkCrops.UpdateCrops(0.5f);
    }

    private void _OnDialogVisualsEvent( String visualsName )
    {
        switch ( visualsName )
        {
            case "plant_crops_light":
                LightCrops.PlantCrops();
                break;
            case "plant_crops_dark":
                DarkCrops.PlantCrops();
                break;
        }
    }

    private void UpdateLightCrops( float oldVal, float newVal )
    {
        LightCrops.UpdateCrops( newVal );
    }
    
    private void UpdateDarkCrops( float oldVal, float newVal )
    {
        DarkCrops.UpdateCrops( newVal );
    }
    
}

} // namespace ComparativeAdvantage