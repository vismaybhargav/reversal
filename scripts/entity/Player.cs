using Godot;
using System;

namespace reversal.scripts.entity;

public partial class Player : CharacterBody2D
{
	[Export] public int Speed { get; set; } = 400;
	
	private Vector2 _screenSize;
	
	[Export]
	public int PlayerSpeedIncrement = 1;

	public override void _Ready()
	{
		// INIT HERE
		_screenSize = GetViewportRect().Size; // we might need this later? lol
		//Position = new Vector2(200, 300);
	}

	public override void _Process(double delta)
	{
		// Rotation
		LookAt(GetGlobalMousePosition());
		
		// WASD movements
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= PlayerSpeedIncrement;
		}
		
		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += PlayerSpeedIncrement;
		}
				
		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= PlayerSpeedIncrement;
		}
						
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += PlayerSpeedIncrement;
		}

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
		}

		Position += velocity * (float)delta;
		
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, _screenSize.X),
			y: Mathf.Clamp(Position.Y, 0, _screenSize.Y)
		);
	}
}
