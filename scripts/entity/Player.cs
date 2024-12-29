using Godot;
using Vector2 = Godot.Vector2;

namespace reversal.scripts.entity;

public partial class Player : Area2D
{
	[Export] public int Speed { get; set; } = 400;
	
	[Export] public int PlayerSpeed = 1;
	
	[Export] public PackedScene BulletScene = GD.Load<PackedScene>("res://scenes/entity/bullet/Bullet.tscn");

	[Export] public float PlayerFireCooldown = 0.25f;
	
	private Vector2 _screenSize;

	private bool _canShoot = true;

	[Signal]
	public delegate void PlayerFiredEventHandler(PackedScene bullet, Vector2 pos, Vector2 dir);

	[Signal]
	public delegate void PlayerHitEventHandler();
	
	// Child Nodes
	private Timer _timer;
	private Marker2D _endOfGun;
	private AnimationPlayer _animationPlayer;
	
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
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public override void _Process(double delta)
	{
		// Rotation
		Rotation = GetAngleToMouseRad();
		
		/*GD.Print($"Player pos: ${Position}");
		GD.Print($"End of Gun Pos${_endOfGun.GlobalPosition}");*/
		
		// WASD movements
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_up")) { velocity.Y -= PlayerSpeed; }
		
		if (Input.IsActionPressed("move_down")) { velocity.Y += PlayerSpeed; }
				
		if (Input.IsActionPressed("move_left")) { velocity.X -= PlayerSpeed; }
						
		if (Input.IsActionPressed("move_right")) { velocity.X += PlayerSpeed; }

		if (velocity.Length() > 0) velocity = velocity.Normalized() * Speed;

		Position += velocity * (float)delta;
		
		if (Input.IsActionJustReleased("shoot")) Shoot();
	}

	private Vector2 ClampPositionToWorldBoundary()
	{
		return new Vector2(
			Mathf.Clamp(Position.X, 0, _screenSize.X),
			Mathf.Clamp(Position.Y, 0, _screenSize.Y)	
		);
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
		
		_animationPlayer.Play("muzzle_flash");
		_canShoot = false;
		_timer.Start(PlayerFireCooldown);
		GD.Print(_endOfGun.Position);
		
		// THE BUG HERE WAS THAT I WAS PASSING IN _endOfGun.Position WHICH IS RELATIVE NOT ABSOLUTE OMG IM DUMBB - VISMAY
		EmitSignal("PlayerFired", BulletScene, _endOfGun.GlobalPosition, Vector2.Right.Rotated(GlobalRotation));
	}

	private void OnCooldownTimeout()
	{
		_canShoot = true;
	}
}
