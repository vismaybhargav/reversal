using Godot;
using reversal.scripts.entity.bullet;
using reversal.scripts.world;

namespace reversal.scripts.entity;

public partial class Player : CharacterBody2D
{
	[Export] public int Speed { get; set; } = 400;
	[Export] public float PlayerFireCooldown = 0.25f;
	[Export] public float PlayerHealCooldown = 5f;
	[Export] public int PlayerHealIncrement = 5;
	[Export] public int MaxPlayerHealth = 200;
	[Export] public PackedScene BulletScene = GD.Load<PackedScene>("res://scenes/entity/bullet/Bullet.tscn");
	[Export] public int ClosedProximityRadius = 10;
	
	private Vector2 _screenSize;
	private int _playerSpeedIncrement  = 1;
	private bool _canShoot = true;
	private bool _canHeal = false;
	private int _health;

	[Signal]
	public delegate void PlayerFiredEventHandler(PackedScene bullet, Vector2 pos, Vector2 dir);

	[Signal]
	public delegate void PlayerHitEventHandler(Bullet bullet);

	[Signal]
	public delegate void PlayerHealthChangedEventHandler(int newHealth);
	
	[Signal]
	public delegate void PlayerDeadEventHandler();
	
	// Child Nodes
	private Timer _timer;
	private Marker2D _endOfGun;
	private AnimationPlayer _animationPlayer;
	private AudioStreamPlayer2D _audioPlayer;
	private AnimatedSprite2D _sprite;
	private Area2D _closeProximityArea;
	private Main.Polarity _polarity;
	
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
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	private void InstantiateNodes()
	{
	}

	public override void _Process(double delta)
	{
		// Rotation
		LookAt(GetGlobalMousePosition());
		
		/*GD.Print($"Player pos: ${Position}");
		GD.Print($"End of Gun Pos${_endOfGun.GlobalPosition}");*/
		
		// WASD movements
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_up")) { velocity.Y -= _playerSpeedIncrement; }
		
		if (Input.IsActionPressed("move_down")) { velocity.Y += _playerSpeedIncrement; }
				
		if (Input.IsActionPressed("move_left")) { velocity.X -= _playerSpeedIncrement; }
						
		if (Input.IsActionPressed("move_right")) { velocity.X += _playerSpeedIncrement; }
		
		if(Input.IsActionJustReleased("ui_accept")) OnPlayerHit(new HeavyBullet());

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			_sprite.Play();
		}

		if (velocity.Length() == 0)
		{
			_sprite.Frame = 0;
			_sprite.Stop();
		}

		Position += velocity * (float)delta;
		Position = ClampPositionToWorldBoundary();

		if (Input.IsActionJustReleased("shoot")) Shoot();
		
		
	}

	private Vector2 ClampPositionToWorldBoundary()
	{
		return new Vector2(
			Mathf.Clamp(Position.X, -1400, 2400),
			Mathf.Clamp(Position.Y, -500, 2250)	
		);
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

	private void OnBodyEntered(Node body)
	{
		if (body is not Enemy enemy) return;
		
		//TODO: This needs to be implemented, this is very wrong right now - VISMAY :sob:
		if (_polarity == Main.Polarity.Positive) enemy.Velocity = -enemy.Velocity;
		else enemy.EnemySpeed *= 2;

	}

	public void OnPlayerHit(Bullet bullet)
	{
		_health -= bullet.Damage;
		_health = Mathf.Clamp(_health, 0, MaxPlayerHealth);

		EmitSignal("PlayerHealthChanged", _health);
		if (_health > 0) return;
		
		EmitSignal("PlayerDead");
		QueueFree();
	}

	private void OnPolaritySwitched(int polarity)
	{
		_polarity = (Main.Polarity)polarity;
	}
}
