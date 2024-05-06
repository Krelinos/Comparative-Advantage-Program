using Godot;
using System;

namespace ComparativeAdvantage
{

public class DefinitionsList : NinePatchRect
{
    private Node DefinitionList;
    private Control DialogBox;
    private RichTextLabel TextLabel;

    private PackedScene _Definition;

    private Godot.Collections.Dictionary Definitions;

    private System.Collections.Generic.List<String> UnlockedDefinitions;

    public override void _Ready()
    {
        DefinitionList = GetNode("MarginContainer/VBoxContainer");
        DialogBox = GetNode<Control>("DefinitionDialogBox");
        TextLabel = GetNode<RichTextLabel>("DefinitionDialogBox/RichTextLabel");

        Definitions = GameService.ParseJSON("definitions.json", "res://Dialog/").Result as Godot.Collections.Dictionary;

        _Definition = GD.Load<PackedScene>("res://Scenes/Definition.tscn");

        UnlockedDefinitions = new System.Collections.Generic.List<String>();

        AppendDefinition("comparative_advantage");
    }

    public void AppendDefinition( String definitionName )
    {
        if ( !UnlockedDefinitions.Contains(definitionName) )
        {
            UnlockedDefinitions.Add(definitionName);

            Button definition = _Definition.Instance() as Button;
            definition.Text = definitionName.Capitalize();
            DefinitionList.AddChild( definition );

            var args = new Godot.Collections.Array( definitionName, definition );
            definition.Connect( "pressed", this, nameof(ShowDefinitionOf), args );
        }
    }

    private void ShowDefinitionOf( String definitionName, Button button )
    {
        var definitionDescription = Definitions[ definitionName ] as String;
        TextLabel.BbcodeText = definitionDescription;
        DialogBox.RectPosition = new Vector2( button.RectSize.x + 15, button.RectPosition.y + 5 );
    }

}

}