using Godot;
using System;

namespace ComparativeAdvantage { namespace mobile {

public class Definition : PanelContainer
{
    [Signal] public delegate void ButtonPressed();

    public Label TermLabel
    {
        get { return _TermLabel; }
        protected set { _TermLabel = value; }
    }

    public Button Button
    {
        get { return _Button; }
        protected set { _Button = value; }
    }

    private Label _TermLabel;
    private Button _Button;

    public override void _Ready()
    {
        TermLabel = GetNode<Label>("MarginContainer/Term");
        Button = GetNode<Button>("Button");

        Button.Connect( "pressed", this, nameof(OnButtonPressed) );
    }

    protected void OnButtonPressed()
    {
        EmitSignal( nameof(ButtonPressed) );
        GD.Print("mrow!");
    }
}

}}