using Godot;
using System;

namespace ComparativeAdvantage
{

public class Choice : MarginContainer, IHasDialogLabel
{
    public int Index
    {
        get { return _Index; }
        set { _Index = value; }
    }

    public CheckBox CheckBox
    {
        get { return CBox; }
        set { }
    }

    private RichTextLabel Label;
    private CheckBox CBox;

    private int _Index;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Label = GetNode<RichTextLabel>("MarginContainer/RichTextLabel");
        CBox = GetNode<CheckBox>("CheckBox");
        _Index = -1;
    }

    public void SetGroup( ButtonGroup bg )
    {
        CBox.Group = bg;
    }

    public void SetLabel( String label )
    {
        Label.Text = Game.Variables.Format( label );
    }
}

}