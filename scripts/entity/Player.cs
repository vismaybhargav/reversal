using Godot;
using System;

namespace reversal.scripts.entity;

public partial class Player : CharacterBody2D
{
	[Export] public int Speed { get; set; } = 400;
	
	public Vector2 ScreenSize;

	public const int PLAYER_SPEED_INCREMENT = 1;

	public override void _Ready()
	{
		// INIT HERE
		ScreenSize = GetViewportRect().Size; // we might need this later? lol
		Position = new Vector2(200, 300);
	}

	public override void _Process(double delta)
	{
		// Rotation
		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 direction = mousePosition - GlobalPosition;
		float angle = Mathf.Atan2(direction.Y, direction.X);

		Rotation = angle;
		
		// WASD movements
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= PLAYER_SPEED_INCREMENT;
		}
		
		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += PLAYER_SPEED_INCREMENT;
		}
				
		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= PLAYER_SPEED_INCREMENT;
		}
						
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += PLAYER_SPEED_INCREMENT;
		}


		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
		}

		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
	}
}
