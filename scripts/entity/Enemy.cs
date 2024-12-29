using Godot;
using reversal.scripts.entity.bullet;

namespace reversal.scripts.entity;

public partial class Enemy : CharacterBody2D 
{
	private int _health = 100;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
}
