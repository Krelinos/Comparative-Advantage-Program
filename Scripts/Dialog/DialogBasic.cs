using Godot;
using System;

namespace ComparativeAdvantage
{

public class DialogBasic : MarginContainer
{
	public String Text
	{
		get { return Label.BbcodeText; }
		set {
			var dialog = Main.Variables.Format( value );
			Label.BbcodeText = dialog;
		}
	}

	protected RichTextLabel Label;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Label = GetNode<RichTextLabel>("MarginContainer/RichTextLabel");
    }
}

} // namespace ComparativeAdvantage