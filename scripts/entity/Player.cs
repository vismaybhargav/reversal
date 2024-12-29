using Godot;
using reversal.scripts.entity.bullet;
using Vector2 = Godot.Vector2;

namespace reversal.scripts.entity;

public partial class Player : Area2D
{
	[Export] public int Speed { get; set; } = 400;
	[Export] public float PlayerFireCooldown = 0.25f;
	[Export] public int MaxPlayerHealth = 200;
	[Export] public PackedScene BulletScene = GD.Load<PackedScene>("res://scenes/entity/bullet/Bullet.tscn");
	
	private Vector2 _screenSize;
	private int _playerSpeedIncrement  = 1;
	private bool _canShoot = true;
	private int _health;

	[Signal]
	public delegate void PlayerFiredEventHandler(PackedScene bullet, Vector2 pos, Vector2 dir);

	[Signal]
	public delegate void PlayerHitEventHandler(Bullet bullet);

	[Signal]
	public delegate void PlayerHealthChangedEventHandler(int newHealth);
	
	// Child Nodes
	private Timer _timer;
	private Marker2D _endOfGun;
	private AnimationPlayer _animationPlayer;
	private AudioStreamPlayer2D _audioPlayer;
	
	public override void _Ready()
	{
		_health = MaxPlayerHealth;
		InstantiateChildNodes();
		// INIT HERE
		_screenSize = GetViewportRect().Size; // we might need this later? lol
	}

	private void InstantiateChildNodes()
	{
		_timer = GetNode<Timer>("Timer");
		_endOfGun = GetNode<Marker2D>("EndOfGun");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_audioPlayer = GetNode<AudioStreamPlayer2D>("BulletSoundPlayer");
	}

	public override void _Process(double delta)
	{
		// Rotation
		Rotation = GetAngleToMouseRad();
		
		/*GD.Print($"Player pos: ${Position}");
		GD.Print($"End of Gun Pos${_endOfGun.GlobalPosition}");*/
		
		// WASD movements
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_up")) { velocity.Y -= _playerSpeedIncrement; }
		
		if (Input.IsActionPressed("move_down")) { velocity.Y += _playerSpeedIncrement; }
				
		if (Input.IsActionPressed("move_left")) { velocity.X -= _playerSpeedIncrement; }
						
		if (Input.IsActionPressed("move_right")) { velocity.X += _playerSpeedIncrement; }
		
		if(Input.IsActionJustReleased("ui_accept")) OnPlayerHit(new HeavyBullet());

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
		_audioPlayer.Play();
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

	private void OnAreaEntered(Area2D area)
	{
		if (area is Bullet bullet)
		{
			EmitSignal("PlayerHit", bullet);
		}
		
	}

	private void OnPlayerHit(Bullet bullet)
	{
		_health -= bullet.Damage;
		EmitSignal("PlayerHealthChanged", _health);
	}
}
