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
    
    public static Glossary SaveInfo
    {
        get { return _SaveInfo; }
        private set { _SaveInfo = value; }
    }

    private static Glossary _Glossary;
    private static Glossary _SaveInfo;


    public override void _EnterTree()
    {
        base._Ready();

        Glossary = new Glossary();
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

public class SaveInfo
{

}

}   // namespace ComparativeAdvantage