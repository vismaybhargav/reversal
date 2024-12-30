using Godot;

namespace reversal.scripts.entity.bullet;
public partial class Bullet : Area2D
{
	[Export] public int   BulletSpeed          = 2500;
	[Export] public float BulletLifeTime       = 0.75f;
	[Export] public int   CameraShakeIntensity = 5;
	[Export] public int   Damage               = 20;
	
	private Vector2 _velocity = Vector2.Zero;

	// Child Nodes
	private Timer _timer;
	
	private void InstantiateChildNodes()
	{
		_timer = GetNode<Timer>("Timer");
	}

	public override void _Ready()
	{
		InstantiateChildNodes();
	}

	public void Start(Vector2 pos, Vector2 dir)
	{
		Position = pos;
		Rotation = dir.Angle();
		_velocity = dir * BulletSpeed;
	}
	
	public void StartFromRotation(Vector2 pos, float rotation)
	{
		Position = pos;
		Rotation = rotation;
		_velocity = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation)) * BulletSpeed;
	}

	public override void _Process(double delta)
	{
		Position += _velocity * (float)delta;
	}

	private void Explode()
	{
		GD.Print("Exploded");
		QueueFree();
	}

	private void OnBodyEntered(Node body)
	{
		switch (body)
		{
			case Player player:
				GD.Print("PLAYER HIT");
				player.OnPlayerHit(this);	
				break;
			case Enemy enemy:
				GD.Print("ENEMY HIT");
				enemy.OnEnemyHit(this);
				break;
		}
		QueueFree();
	}
}
