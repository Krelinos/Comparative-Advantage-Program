using Godot;
using System;

namespace ComparativeAdvantage
{

public class Main : Control
{
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

    private static Glossary _Glossary;
    private static SaveInfo _SaveInfo;

    public override void _EnterTree()
    {
        base._Ready();

        Glossary = new Glossary();
        SaveInfo = new SaveInfo();
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