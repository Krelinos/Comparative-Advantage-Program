using ComparativeAdvantage;
using Godot;
using System;
using System.Runtime.CompilerServices;

public class Settings : MarginContainer
{
    [Export] private NodePath _Seed;
    [Export] private NodePath _ResetScenario;
    [Export] private NodePath _ResetGlossary;
    [Export] private NodePath _ResetPractice;
    [Export] private NodePath _Apply;

    private LineEdit Seed;
    private Button ResetScenario;
    private Button ResetGlossary;
    private Button ResetPractice;
    private Button Apply;

    // Scenario is 0b001;
    // Glossary is 0b010;
    // Practive is 0b100;
    private byte ResetPendingFlags = 0b000;

    private const String CONFIRMATION_TEXT_MOBILE = "Tap the Apply button to confirm";
    private const String CONFIRMATION_TEXT_DESKTOP = "Click the Apply button to confirm";

    public override void _Ready()
    {
        Seed = GetNode<LineEdit>( _Seed );
        ResetScenario = GetNode<Button>( _ResetScenario );
        ResetGlossary = GetNode<Button>( _ResetGlossary );
        ResetPractice = GetNode<Button>( _ResetPractice );
        Apply = GetNode<Button>( _Apply );

        Connect( "hide", this, nameof(OnHide) );
        ResetScenario.Connect( "pressed", this, nameof(Select),
            new Godot.Collections.Array { ResetScenario, 0b1 } );
        ResetGlossary.Connect( "pressed", this, nameof(Select),
            new Godot.Collections.Array { ResetGlossary, 0b10 } );
        ResetPractice.Connect( "pressed", this, nameof(Select),
            new Godot.Collections.Array { ResetPractice, 0b100 } );
        Apply.Connect( "pressed", this, nameof(OnApplyPressed) );
    }

    private void Select( Button button, byte flag )
    {
        button.Disabled = true;
        button.Text = Main.IsClientOnDesktop ? CONFIRMATION_TEXT_DESKTOP : CONFIRMATION_TEXT_MOBILE;
        ResetPendingFlags |= flag;
    }

    private void OnApplyPressed()
    {
        if ( (ResetPendingFlags & 0b001) == 0b1 )
            Main.SaveInfo.ResetScenarioProgress();
        if ( (ResetPendingFlags & 0b010) == 0b10 )
        {
            Main.SaveInfo.ResetLearnedTerms();
            Main.Glossary.Reset();
        }
        if ( (ResetPendingFlags & 0b100) == 0b100 )
            Main.SaveInfo.ResetPracticeCount();
        
        Apply.Text = "Actions applied!";
    }

    private void OnHide()
    {
        ResetScenario.Text = ResetScenario.GetParent().Name.Capitalize();
        ResetGlossary.Text = ResetGlossary.GetParent().Name.Capitalize();
        ResetPractice.Text = ResetPractice.GetParent().Name.Capitalize();

        ResetScenario.Disabled = false;
        ResetGlossary.Disabled = false;
        ResetPractice.Disabled = false;

        Apply.Text = "Apply";
        ResetPendingFlags = 0;
    }
}
