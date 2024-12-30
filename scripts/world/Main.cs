using System;
using Godot;
using Godot.Collections;
using reversal.scripts.entity;
using reversal.scripts.entity.bullet;
using reversal.scripts.util;

namespace reversal.scripts.world;

public partial class Main : Node2D
{
	[Export] public int EnemyCount = 5;
	[Export] public float EnemySpawnMinY = -450f;
	[Export] public float EnemySpawnMaxY = 2240f;
	
	private CharacterBody2D _player;
	
	private Camera2D _camera;
	private CanvasLayer _dbgUi;
	
	private void InstantiateChildNodes()
	{
		_player = GetNode<CharacterBody2D>("Player");
		_camera = GetNode<Camera2D>("Player/PlayerCamera");
		_dbgUi = GetNode<CanvasLayer>("DBG_Info");
	}
	
	public override void _Ready()
	{
		GD.Print("Main ready");
		InstantiateChildNodes();
		
		if(!OS.IsDebugBuild()) _dbgUi.Hide();
		
		_camera.MakeCurrent();

		for (var _ = 0; _ < EnemyCount; ++_) SpawnEnemy();
	}

	private void SpawnEnemy()
	{
		var enemyScene = GD.Load<PackedScene>("res://scenes/entity/enemy/Enemy.tscn");
		var enemy = (Enemy)enemyScene.Instantiate();
		AddChild(enemy);
		
		var random = new RandomNumberGenerator();
		random.Randomize();
		
		Array<int> spawnZonesX = [-1440, -800, 1920, 1700];
		var spawnX = spawnZonesX[random.RandiRange(0, spawnZonesX.Count - 1)];
		
		random.Randomize();
		var spawnY = (int)random.RandfRange(EnemySpawnMinY, EnemySpawnMaxY);
		
		enemy.Position = new Vector2(spawnX, spawnY);
	}
	
	private void OnPlayerShoot(PackedScene bullet, Vector2 pos, Vector2 dir)
	{
		var b = (Bullet)bullet.Instantiate();
		AddChild(b);
		b.Start(pos, dir);
	}
}
