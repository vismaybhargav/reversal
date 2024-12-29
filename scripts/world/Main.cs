using System;
using Godot;
using reversal.scripts.entity.bullet;

namespace reversal.scripts.world;

public partial class Main : Node2D
{
	private Area2D _player;
	
	private Camera2D _camera;
	private CanvasLayer _dbgUi;
	
	private void InstantiateChildNodes()
	{
		_player = GetNode<Area2D>("Player");
		_camera = GetNode<Camera2D>("Player/PlayerCamera");
		_dbgUi = GetNode<CanvasLayer>("DBG_Info");
	}
	
	public override void _Ready()
	{
		GD.Print("Main ready");
		InstantiateChildNodes();
		
		if(!OS.IsDebugBuild()) _dbgUi.Hide();
		
		_camera.MakeCurrent();
	}
	
	

	private void OnPlayerShoot(PackedScene bullet, Vector2 pos, Vector2 dir)
	{
		var b = (Bullet)bullet.Instantiate();
		AddChild(b);
		b.Start(pos, dir);
	}
}
