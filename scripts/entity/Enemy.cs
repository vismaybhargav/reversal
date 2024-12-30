using Godot;
using reversal.scripts.entity.bullet;

namespace reversal.scripts.entity;

public partial class Enemy : CharacterBody2D
{
	[Export] public int EnemySpeed = 35;
	private const int EnemyHealth = 100;
	[Export] public float EnemyFireCooldown = 1f;
	[Export] public float EnemyBulletDeviationRadians = 0.3f;
	
	[Export] public PackedScene BulletScene = GD.Load<PackedScene>("res://scenes/entity/bullet/Bullet.tscn");
	
	[Signal]
	public delegate void EnemyFiredEventHandler(PackedScene bullet, Vector2 pos, Vector2 dir);
	
	private int _health = EnemyHealth;
	private bool _canShoot = true;
	private bool _inLineOfSight = false;
	private bool _isPushedBack = false;
	private float _pushbackDuration = 0.5f;
	private float _pushbackTimer = 0.0f;
	private float _pushbackForce = 200.0f;	
	
	private RandomNumberGenerator _rand = new();

	private Area2D _lineOfSight;
	private CharacterBody2D _player;
	
	private Timer _timer;
	private Timer _colorResetTimer;
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
		_colorResetTimer = GetNode<Timer>("ColorResetTimer");
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
		_sprite.Modulate = new Color(1, 0, 0, 1);
		_colorResetTimer.Start(0.1f);
		
		GD.Print("enemy hit! ", _health);
		if (_health <= 0) QueueFree();
	}
	
	// TERRIBLE name ngl
	private void OnPlayerInLineOfSight(Node body)
	{
		if (body is not Player) return;
		GD.Print("Player In LineOfSight");
		_inLineOfSight = true;
	}

	private void OnPlayerOutOfLineOfSight(Node body)
	{
		if (body is not Player) return;
		GD.Print("Player out of LineOfSight");
		_inLineOfSight = false;
	}
	
	private void Shoot()
	{
		if (!_canShoot) return;

		_animationPlayer.Play("muzzle_flash");

		var b = (Bullet)BulletScene.Instantiate();
		AddChild(b);

		b.StartFromRotation(_endOfGun.Position, _endOfGun.Rotation + GenerateRandomDeviation());

		_audioPlayer.Play();
		_canShoot = false;
		_timer.Start(EnemyFireCooldown);
		GD.Print(_endOfGun.GlobalPosition);
	}
	
	private void OnCooldownTimeout()
	{
		_canShoot = true;
	}

	private void OnColorResetTimeout()
	{
		_sprite.Modulate = new Color(1, 1, 1, 1);	
	}

	private float GenerateRandomDeviation()
	{
		return _rand.RandfRange(-EnemyBulletDeviationRadians, EnemyBulletDeviationRadians);

	}
}
