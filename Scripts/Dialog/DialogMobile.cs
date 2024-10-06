using ComparativeAdvantage;
using Godot;
using System;
using System.Collections.Generic;

public class DialogMobile : Dialog
{
    protected const String CONTINUE_CONTINUE_MOBILE = "Tap here to continue.";

    public override void _Ready()
    {
        base._Ready();
    }

    public new void Resume()
    {
        IsDialogPaused = false;
        ContinueButton.Disabled = false;
        ContinueButton.Text = CONTINUE_CONTINUE_MOBILE;
    }
}
