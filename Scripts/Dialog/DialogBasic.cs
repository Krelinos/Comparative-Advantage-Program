using Godot;
using System;

namespace ComparativeAdvantage
{

public class DialogBasic : MarginContainer, IHasDialogLabel
{
	protected RichTextLabel Label;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Label = GetNode<RichTextLabel>("MarginContainer/RichTextLabel");
    }

	public void SetLabel( String dialog )
	{
		dialog = Game.Variables.Format( dialog );
		Label.BbcodeText = dialog;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

}
