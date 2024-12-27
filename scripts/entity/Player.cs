using Godot;
using System;

namespace reversal.scripts.entity;

public partial class Player : CharacterBody2D
{
	public Vector2 ScreenSize;

	public override void _Ready()
	{
		// INIT HERE
		ScreenSize = GetViewportRect().Size; // we might need this later? lol
	}

	public override void _Process(double delta)
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 direction = mousePosition - GlobalPosition;
		float angle = Mathf.Atan2(direction.Y, direction.X);

		Rotation = angle;
	}
}
