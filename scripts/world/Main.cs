using Godot;
using reversal.scripts.entity.bullet;

namespace reversal.scripts.world;

public partial class Main : Node2D
{
	private CharacterBody2D _player;
	private Camera2D _camera;

	public override void _Ready()
	{
		GD.Print("Main ready");
		InstantiateChildNodes();
		
		_camera.MakeCurrent();

		//_bulletManager = GetNode<BulletManager>("BulletManager");
		//_player.Connect("PlayerFired", new Callable(_bulletManager, nameof(_bulletManager.HandleBulletSpawned)));
	}
	
	/// <summary>
	/// Instantiates all the child nodes
	/// </summary>
	private void InstantiateChildNodes()
	{
		_player = GetNode<CharacterBody2D>("Player");
		_camera = GetNode<Camera2D>("Player/Camera2D");
	}

	private void OnPlayerShoot(PackedScene bullet, Vector2 pos, Vector2 dir)
	{
		var b = (Bullet)bullet.Instantiate();
		AddChild(b);
		b.Start(pos, dir);
	}
}
