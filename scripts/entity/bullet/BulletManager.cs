using Godot;

namespace reversal.scripts.entity.bullet;

public partial class BulletManager : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void HandleBulletSpawned(Bullet bullet, Vector2 pos)
	{
		GD.Print("Bullet spawned");
		AddChild(bullet);
		GlobalPosition = pos;
	}
}
