using Godot;
using System;
using System.Collections.Generic;

namespace ComparativeAdvantage
{

public class ScenarioService
{
    private Godot.Collections.Dictionary Dialog;
    private Godot.Collections.Dictionary Questions;
    public String CurrentScenario { get; private set; }
    public Node CurrentScenarioVisuals { get; private set; }
	public Node CurrentScenarioUI { get; private set; }
    
    public void LoadScenario( String scenarioName )
    {
        CurrentScenario = scenarioName;

        Godot.Collections.Dictionary scenarioInfo = Game.ParseJSON( $"{scenarioName}.json", "res://Dialog/" ).Result as Godot.Collections.Dictionary;
        Dialog = scenarioInfo[ "dialog" ] as Godot.Collections.Dictionary;
        Questions = scenarioInfo[ "questions" ] as Godot.Collections.Dictionary;

        CurrentScenarioVisuals = GD.Load<PackedScene>("res://Scenes/Scenarios/Visuals/Scenario1aOutput.tscn").Instance();
        CurrentScenarioUI = GD.Load<PackedScene>("res://Scenes/Scenarios/UI/Scenario1aOutput.tscn").Instance();
    }

    public Godot.Collections.Dictionary GetDialog( String dialogId )
    {
        return Dialog[ dialogId ] as Godot.Collections.Dictionary;
    }

    public Godot.Collections.Dictionary GetQuestion( String questionId )
    {
        return Questions[ questionId ] as Godot.Collections.Dictionary;
    }

    public String GetStartID()
    {
        return Dialog[ "start" ] as String;
    }
}

}