using Godot;
using System;

public class TouchSwipeScrollContainer : ScrollContainer
{
	protected Vector2 TouchStartPos;
	protected int VScrollInitial;
	protected float Velocity;

	public override void _Ready()
	{
	}

    public override void _Process(float delta)
    {
        base._Process(delta);

		ScrollVertical += (int)Velocity;

		Velocity = Mathf.Lerp( Velocity, 0, 0.1f );
    }

    public override void _GuiInput(InputEvent @event)
	{
		base._Input(@event);

		if ( @event is InputEventScreenTouch touch )
		{
			TouchStartPos = touch.Position;
			VScrollInitial = ScrollVertical;
		}

		if ( @event is InputEventScreenDrag drag )
		{
			ScrollVertical = VScrollInitial - (int)(drag.Position - TouchStartPos).y;
		}
	}
}
