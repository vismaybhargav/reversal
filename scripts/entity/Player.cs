using Godot;
using System;

namespace reversal.scripts.entity;

public partial class Player : CharacterBody2D
{
	[Export] public int Speed { get; set; } = 400;
	
	private Vector2 _screenSize;
	
	[Export]
	public int PlayerSpeedIncrement = 1;
	
	// Child Nodes
	private CollisionShape2D _collisionShape;

	public override void _Ready()
	{
		InstantiateNodes();
		
		// INIT HERE
		_screenSize = GetViewportRect().Size; // we might need this later? lol
		//Position = new Vector2(200, 300);
	}

	private void InstantiateNodes()
	{
		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
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
	
	/// <summary>
	/// Checks if the collision shape of the player contains the vector
	/// </summary>
	/// <param name="vec2">the vector to check</param>
	/// <returns>true if the vector is in the shape, false otherwise</returns>
	private bool DoesShapeContain(Vector2 vec2)
	{
		var shape = (RectangleShape2D)_collisionShape.Shape;
		
		//TODO: Fix this math
		return IsBetween(vec2.X, _collisionShape.Position.X + shape.Size.X, _collisionShape.Position.X) && 
		   IsBetween(vec2.Y,  _collisionShape.Position.Y + shape.Size.Y, _collisionShape.Position.Y);
	}
	
	/// <summary>
	/// Checks if the provided value is in between max and min
	/// </summary>
	/// <param name="val">Provided value</param>
	/// <param name="max">Max value</param>
	/// <param name="min">Minimum Value</param>
	/// <returns> max lessThanEqual val greaterThanEqual min </returns>
	private static bool IsBetween(double val, double max, double min)
	{
		return val >= min && val <= max;

	}
}
