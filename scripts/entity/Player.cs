using Godot;

namespace reversal.scripts.entity;

public partial class Player : CharacterBody2D
{
	[Export] 
	public int Speed { get; set; } = 400;
	
	[Export]
	public int PlayerSpeedIncrement = 1;
	
	[Export]
	PackedScene _bulletScene = GD.Load<PackedScene>("res://scenes/Bullet.tscn");
	
	private Vector2 _screenSize;

	private bool _canShoot = true;

	[Signal]
	public delegate void PlayerFiredEventHandler(PackedScene bullet, Vector2 pos, Vector2 dir);
	
	// Child Nodes
	private Timer _timer;
	private Marker2D _endOfGun;
	
	public override void _Ready()
	{
		InstantiateChildNodes();
		// INIT HERE
		_screenSize = GetViewportRect().Size; // we might need this later? lol
	}

	private void InstantiateChildNodes()
	{
		_timer = GetNode<Timer>("Timer");
		_endOfGun = GetNode<Marker2D>("EndOfGun");
	}

	public override void _Process(double delta)
	{
		// Rotation
		Rotation = GetAngleToMouseRad();
		
		/*GD.Print($"Player pos: ${Position}");
		GD.Print($"End of Gun Pos${_endOfGun.GlobalPosition}");*/
		
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
		
		if (Input.IsActionJustReleased("shoot"))
		{
			Shoot();
		}
	}

	/// <summary>
	/// Calculates the angle between current rotation and the mouse
	/// </summary>
	/// <returns>the angle in radians</returns>
	private float GetAngleToMouseRad()
	{
		var direction = GetGlobalMousePosition() - GlobalPosition;
		return Mathf.Atan2(direction.Y, direction.X);	
	}

	private void Shoot()
	{
		if (!_canShoot) return;
		
		_canShoot = false;
		_timer.Start();
		GD.Print(_endOfGun.Position);
		
		// THE BUG HERE WAS THAT I WAS PASSING IN _endOfGun.Position WHICH IS RELATIVE NOT ABSOLUTE OMG IM DUMBB - VISMAY
		EmitSignal("PlayerFired", _bulletScene, _endOfGun.GlobalPosition, Vector2.Right.Rotated(GlobalRotation));
	}

	private void OnCooldownTimeout()
	{
		_canShoot = true;
	}
}
