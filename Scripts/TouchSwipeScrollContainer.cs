using Godot;
using System;

/// <summary>
/// A ScrollContainer that responds to touchscreen InputEvents. Swiping quickly
/// will continue scrolling the scrollbar as is typical of mobile applications.
/// </summary>
public class TouchSwipeScrollContainer : ScrollContainer
{
	protected Vector2 TouchStartPos;
	protected int VScrollInitial;
	protected float Velocity;

	protected Vector2 LastDragSpeed;
	protected float TimeSinceLastDragEvent;

	public override void _Ready()
	{
		// Since this is touchscreen, scrolling via scrollbar is not required.
		GetVScrollbar().MouseFilter = MouseFilterEnum.Ignore;
	}

    public override void _Process(float delta)
    {
        base._Process(delta);
		
		if ( Math.Abs(400*delta) < Math.Abs(Velocity) )
			Velocity -= 400*delta * Math.Sign(Velocity);
		else
			Velocity -= Velocity;

		ScrollVertical -= (int)(Velocity*0.1f);
		TimeSinceLastDragEvent += delta;
    }

    public override void _GuiInput(InputEvent @event)
	{
		base._Input(@event);

		if ( @event is InputEventScreenTouch touch )
		{
			if ( touch.Pressed )
			{
				TouchStartPos = touch.Position;
				VScrollInitial = ScrollVertical;
				Velocity = 0;
			}
			else
				Velocity = TimeSinceLastDragEvent > 0.1f ? 0 : LastDragSpeed.y*0.35f;
		}

		if ( @event is InputEventScreenDrag drag )
		{
			TimeSinceLastDragEvent = 0;

			ScrollVertical = VScrollInitial + (int)(TouchStartPos - drag.Position).y;
			LastDragSpeed = drag.Speed;
		}
	}
}
