using ComparativeAdvantage;
using Godot;
using System;

public class Scenario1aOutputVisuals : Node
{
    private Scenario1aOutputUI UI;

    private FarmCrops LightCrops;
    private FarmCrops DarkCrops;

    public override void _Ready()
    {
        LightCrops = GetNode<FarmCrops>("LightCrops");
        DarkCrops = GetNode<FarmCrops>("DarkCrops");
    }

    public void Initialize()
    {
        UI = GameService.Scenario.CurrentScenarioUI as Scenario1aOutputUI;
        UI.Connect( "LightCropChanged", this, nameof(UpdateLightCrops) );
        UI.Connect( "DarkCropChanged", this, nameof(UpdateDarkCrops) );

        GameService.Dialog.Connect("DialogVisualsEvent", this, nameof(_OnDialogVisualsEvent));
    
        LightCrops.Initialize( new Vector2( 20, 0 ), new Vector2( 10, 20 ), 4, 3, 2, 4 );
        LightCrops.UpdateCrops(0.5f);
        DarkCrops.Initialize( new Vector2( 12.5f, 0 ), new Vector2( 7.5f, 20 ), 6, 3, 8, 16 );
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
