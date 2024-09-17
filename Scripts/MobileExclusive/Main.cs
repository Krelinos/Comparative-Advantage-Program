using Godot;
using System;
using System.Data.SqlTypes;
using System.Dynamic;

namespace ComparativeAdvantage
{

public class Main : Control
{
    [Export] private NodePath _ScenarioSelectionButtons;
    [Export] private NodePath _ScenarioVisualsParent;
    [Export] private NodePath _ScenarioUIParent;

    public static Scenarios Scenarios
    {
        get { return _Scenarios; }
        private set { _Scenarios = value; }
    }

    public static Glossary Glossary
    {
        get { return _Glossary; }
        private set { _Glossary = value; }
    }
    
    public static SaveInfo SaveInfo
    {
        get { return _SaveInfo; }
        private set { _SaveInfo = value; }
    }

    public bool IsClientOnDesktop = false;

    private static Scenarios _Scenarios;
    private static Glossary _Glossary;
    private static SaveInfo _SaveInfo;

    private Node ScenarioVisualsParent;
    private Node ScenarioUIParent;
    private Node ScenarioVisuals;
    private Node ScenarioUI;

    public override void _EnterTree()
    {
        base._Ready();

        Scenarios = new Scenarios( GetNode( _ScenarioSelectionButtons) );
        Glossary = new Glossary();
        SaveInfo = new SaveInfo();

        ScenarioVisualsParent = GetNode( _ScenarioVisualsParent );
        ScenarioUIParent = GetNode( _ScenarioUIParent );

        Scenarios.Connect( nameof(Scenarios.ScenarioLoaded), this, nameof(OnScenarioLoaded) );
    }

    public static JSONParseResult ParseJSON( String fileName, String filePath )
	{
		// fileName = fileName.ToLower();
		File JsonFile = new File();
		
		// 28 Sept 2023 - ModeFlag is Case-Sensitive; .READ will error, but .Read is fine
		// Probably a C# thing. This may apply to other Enums as well.
		Error e = JsonFile.Open($"{filePath}{fileName}", File.ModeFlags.Read);
		
		if ( e != Error.Ok )
			GD.PushError( String.Format("Tried to open file '{0}', but received error '{1}'.", fileName, e.ToString()) );
		
		JSONParseResult json = JSON.Parse( JsonFile.GetAsText() );
		JsonFile.Close();
		
		if ( json.Error != Error.Ok )
			GD.PushError( String.Format("Tried to parse JSON of file '{0}', but received error '{1}'.", fileName, json.Error.ToString()) );
	
		return json;
	}

    private void OnScenarioLoaded( String scenarioName )
    {
        ScenarioVisuals?.QueueFree();
        ScenarioUI?.QueueFree();

        var _ScenarioVisuals = GD.Load(
            "res://Scenes/Scenarios/"
            + ( IsClientOnDesktop ? "Desktop" : "Mobile")
            + "/Visuals" + scenarioName + ".tscn"
        ) as PackedScene;
        ScenarioVisuals = _ScenarioVisuals.Instance();

        var _ScenarioUI = GD.Load(
            "res://Scenes/Scenarios/"
            + ( IsClientOnDesktop ? "Desktop" : "Mobile")
            + "/UI" + scenarioName + ".tscn"
        ) as PackedScene;
        ScenarioUI = _ScenarioUI.Instance();

        ScenarioVisualsParent.AddChild( ScenarioVisuals );
        ScenarioUIParent.AddChild( ScenarioUI );
    }
}


/// <summary>
/// Handles scenario selection and the dialog, term discovery, and questions
/// as a user progresses through a scenario.
/// </summary>
public class Scenarios : Godot.Object
{
    [Signal] public delegate void ScenarioLoaded( String scenarioName );

    public Godot.Collections.Array Dialog { get; private set; }
    public Godot.Collections.Dictionary Questions { get; private set; }

    private int NextDialogIndex;

    public Scenarios( Node scenarioButtonsList )
    {
        foreach( Node n in scenarioButtonsList.GetChildren() )
        {
            if ( n is BaseButton b )
            {
                b.Connect( "pressed", this, nameof(LoadScenario) );
            }
        }
    }

    private void LoadScenario( String scenarioFileName )
    {
        var scenarioData = Main.ParseJSON( scenarioFileName, "res://Dialog/" ).Result as Godot.Collections.Dictionary;
    
        Dialog = scenarioData["dialog"] as Godot.Collections.Array;
        Questions = scenarioData["questions"] as Godot.Collections.Dictionary;

        EmitSignal( nameof(ScenarioLoaded), scenarioFileName );
    }

    public Godot.Collections.Dictionary PopDialog()
    {
        return Dialog[ NextDialogIndex++ ] as Godot.Collections.Dictionary;
    }
}

public class Glossary
{
    public Godot.Collections.Dictionary TermDescriptions { get; protected set; }

    public Glossary()
    {
        TermDescriptions = Main.ParseJSON("definitions.json", "res://Dialog/").Result
            as Godot.Collections.Dictionary;
    }
}

/// <summary>
/// Reads and writes user save data to a file.
/// </summary>
public class SaveInfo
{
    public Godot.Collections.Dictionary Data { get; protected set; }

    protected const String SAVE_FILE_PATH = "userdata.json";

    public SaveInfo()
    {
        Load();
    }

    public void Save()
    {
        Godot.File saveFile = new Godot.File();
        String saveJSON = JSON.Print( Data, "\t" );

        saveFile.Open("user://" + SAVE_FILE_PATH, File.ModeFlags.Write);
        saveFile.StoreString( saveJSON );
        saveFile.Close();
    }

    public void Load()
    {
        Godot.File saveFile = new Godot.File();
        if ( saveFile.FileExists("user://" + SAVE_FILE_PATH) )
            Data = GameService.ParseJSON( SAVE_FILE_PATH, "user://" ).Result as Godot.Collections.Dictionary;
        else    // Nonexistant save file means this is a new user.
        {
            Data = new Godot.Collections.Dictionary {};
            ResetScenarioProgress();
            ResetGlossary();

            saveFile.Open("user://" + SAVE_FILE_PATH, File.ModeFlags.Write);
            saveFile.StoreString( JSON.Print(Data, "\t") );
            saveFile.Close();
        }
    }

    public void ResetScenarioProgress()
    {
        Data["solved"] = new Godot.Collections.Dictionary
        {
            { "Scenario1aOutput", new Godot.Collections.Array() },
            { "Scenario1bInput", new Godot.Collections.Array() }
        };
    }

    public void ResetGlossary()
    {
        Data["terms"] = new Godot.Collections.Array();
    }
}

}   // namespace ComparativeAdvantage