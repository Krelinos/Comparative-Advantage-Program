using Godot;
using System;

namespace ComparativeAdvantage
{

public class DefinitionsList : NinePatchRect
{
    private Control DefinitionList;
    private Control DialogBox;
    private RichTextLabel TextLabel;

    private PackedScene _Definition;

    private Godot.Collections.Dictionary Definitions;

    private System.Collections.Generic.List<String> UnlockedDefinitions;

    public override void _Ready()
    {
        DefinitionList = GetNode<Control>("MarginContainer/VBoxContainer");
        DialogBox = GetNode<Control>("../../../../DefinitionDialogBox");
        TextLabel = DialogBox.GetNode<RichTextLabel>("RichTextLabel");

        Definitions = GameService.ParseJSON("definitions.json", "res://Dialog/").Result as Godot.Collections.Dictionary;

        _Definition = GD.Load<PackedScene>("res://Scenes/Definition.tscn");

        UnlockedDefinitions = new System.Collections.Generic.List<String>();
    }

    public void AppendDefinition( String definitionName )
    {
        if ( !UnlockedDefinitions.Contains(definitionName) )
        {
            UnlockedDefinitions.Add(definitionName);

            Definition definition = _Definition.Instance() as Definition;
            definition.Text = definitionName.Capitalize();
            DefinitionList.AddChild( definition );
            definition.Initialize();

            var args = new Godot.Collections.Array( definitionName, definition );
            definition.Connect( "mouse_entered", this, nameof(ShowDefinitionOf), args );
            definition.Connect("mouse_exited", this, nameof(HideDefinitionDescription));
        }
    }

    private void ShowDefinitionOf( String definitionName, Button button )
    {
        DialogBox.Visible = true;
        var definitionDescription = Definitions[ definitionName ] as String;
        TextLabel.BbcodeText = definitionDescription;
        DialogBox.RectPosition = new Vector2( button.RectSize.x + 15, button.RectPosition.y + 8 );
    }

    private void HideDefinitionDescription()
    {
        DialogBox.Visible = false;
    }

}

}