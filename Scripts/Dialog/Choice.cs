using Godot;
using System;

namespace ComparativeAdvantage
{

public class Choice : MarginContainer
{
    public int Index
    {
        get { return _Index; }
        set { _Index = value; }
    }

    public CheckBox CheckBox
    {
        get { return _CheckBox; }
    }
    
    public TextureRect CheckBoxIcon
    {
        get { return _CheckBoxIcon; }
    }

    public ButtonGroup ButtonGroup
    {
        get { return CheckBox.Group; }
        set { CheckBox.Group = value; }
    }

    public String LabelText
    {
        get { return _Label.Text; }
        set { _Label.Text = GameService.Variables.Format( value ); }
    }

    protected int _Index;
    protected RichTextLabel _Label;
    protected CheckBox _CheckBox;
    protected TextureRect _CheckBoxIcon;

    protected static AtlasTexture CheckBoxToggledOn;
    protected static AtlasTexture CheckBoxToggledOff;

    static Choice()
    {
        CheckBoxToggledOn = GD.Load("res://Assets/CheckBoxIconOn.tres") as AtlasTexture;
        CheckBoxToggledOff = GD.Load("res://Assets/CheckBoxIconOff.tres") as AtlasTexture;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _Index = -1;
        _Label = GetNode<RichTextLabel>("HBoxContainer/RichTextLabel");
        _CheckBoxIcon = GetNode<TextureRect>("HBoxContainer/TextureRect");
        _CheckBox = GetNode<CheckBox>("HBoxContainer/TextureRect/CheckBox");

        CheckBox.Connect( "toggled", this, nameof(OnCheckBoxToggled) );
    }

    protected void OnCheckBoxToggled( bool isToggledOn )
    {
        if ( isToggledOn )
            CheckBoxIcon.Texture = CheckBoxToggledOn;
        else
            CheckBoxIcon.Texture = CheckBoxToggledOff;
    }
}

}   // namespace ComparativeAdvantage.Dialog