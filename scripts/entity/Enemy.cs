using Godot;
using reversal.scripts.entity.bullet;

namespace reversal.scripts.entity;

public partial class Enemy : CharacterBody2D
{
	[Export] public int EnemySpeed = 35;
	private const int EnemyHealth = 100;
	[Export] public float EnemyFireCooldown = 1f;
	
	[Export] public PackedScene BulletScene = GD.Load<PackedScene>("res://scenes/entity/bullet/Bullet.tscn");
	
	[Signal]
	public delegate void EnemyFiredEventHandler(PackedScene bullet, Vector2 pos, Vector2 dir);
	
	private int _health = EnemyHealth;
	private bool _canShoot = true;
	private bool _inLineOfSight = false;

	private Area2D _lineOfSight;
	private CharacterBody2D _player;
	
	private Timer _timer;
	private Marker2D _endOfGun;
	private AnimationPlayer _animationPlayer;
	private AudioStreamPlayer2D _audioPlayer;
	private AnimatedSprite2D _sprite;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InstantiateChildNodes();
		InstantiateNodes();
	}

	private void InstantiateNodes()
	{
		_player = GetParent().GetNode<CharacterBody2D>("Player");
	}
	
	private void InstantiateChildNodes()
	{
		_lineOfSight = GetNode<Area2D>("LineOfSight");
		
		_timer = GetNode<Timer>("Timer");
		_endOfGun = GetNode<Marker2D>("EndOfGun");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_audioPlayer = GetNode<AudioStreamPlayer2D>("BulletSoundPlayer");
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		MoveTowards(_player.GlobalPosition, delta);

		if (_inLineOfSight)
		{
			Shoot();
		}
	}

	private void MoveTowards(Vector2 target, double delta)
	{
		Vector2 direction = (target - Position).Normalized();
		Vector2 velocity = direction * EnemySpeed;
		Position += velocity * (float)delta;
		
		Rotation = Mathf.Atan2(direction.Y, direction.X);

		// Optional: Stop moving if close enough to the target
		// if (Position.DistanceTo(target) < 1.0f)
		// {
		// 	Position = target;
		// }	
	}
	
	public void OnEnemyHit(Bullet bullet)
	{
		_health -= bullet.Damage;
		//EmitSignal("PlayerHealthChanged", _health);
		GD.Print("enemy hit! ", _health);
		if (_health <= 0)
		{
			QueueFree();
		}
	}
	
	// TERRIBLE name ngl
	private void OnPlayerInLineOfSight(Node body)
	{
		if (body is Player player)
		{
			GD.Print("Player In LineOfSight");
			_inLineOfSight = true;
		}
	}

	private void OnPlayerOutOfLineOfSight(Node body)
	{
		if (body is Player player)
		{
			GD.Print("Player out of LineOfSight");
			_inLineOfSight = false;
		}
	}
	
	private void Shoot()
	{
		if (!_canShoot) return;
		
		_animationPlayer.Play("muzzle_flash");
			
		PackedScene bullet = GD.Load<PackedScene>("res://scenes/entity/bullet/Bullet.tscn");
		
		var b = (Bullet)bullet.Instantiate();
		AddChild(b);
		b.Start(_endOfGun.Position, Vector2.Right.Rotated(GlobalRotation));
		
		_audioPlayer.Play();
		_canShoot = false;
		_timer.Start(EnemyFireCooldown);
		GD.Print(_endOfGun.Position);
	}
	
	
	private void OnCooldownTimeout()
	{
		_canShoot = true;
	}

	// private void OnAreaEntered(Area2D area)
	// {
	// 	if (area is Bullet bullet)
	// 	{
	// 		EmitSignal("PlayerHit", bullet);
	// 	}
	// }
}
