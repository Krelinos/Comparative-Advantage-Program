using System;
using Godot;

namespace ComparativeAdvantage { namespace mobile {

/// <summary>
/// For a side menu that has a submenu that slides in. Currently, there is only
/// only side menu that does that, so I am not too bothered that it is tailor-made
/// for that one. I doubt it would change, but if it does... hello future me!
/// </summary>
public class SideMenuWithExtra : SideMenu
{
    private Control TermDetails;
    private Control Offscreen;

    public override void _Ready()
    {
        base._Ready();
        TermDetails = GetNode<Control>("TermDetails");
        Offscreen = GetNode<Control>("Offscreen");
    }

    public new void OnScreenResized()
    {
        base.OnScreenResized();
        TermDetails.RectPosition = new Vector2( Offscreen.RectSize.x, 0 );
        GD.Print( Offscreen.RectSize.x );
    }
}

} // namespace mobile
} // namespace ComparativeAdvantage