using System;
using Godot;
using Godot.Collections;
using reversal.scripts.entity;
using reversal.scripts.entity.bullet;

namespace reversal.scripts.world;

public partial class Main : Node2D
{
	public enum Polarity
	{
		Positive,
		Negative
	}

	[Export] public int EnemyCount = 5;
	[Export] public float EnemySpawnMinY = -450f;
	[Export] public float EnemySpawnMaxY = 2240f;
	[Export] public int PolaritySwitchDuration = 30;

	[Signal]
	public delegate void
		PolaritySwitchEventHandler(int polarity); // 0 = Positive, 1 = Negative Godot being annoying with enums

	public Polarity CurrentPolarity = Polarity.Positive;

	private CharacterBody2D _player;
	private Timer _polaritySwitchCountdown;
	private Camera2D _camera;
	private CanvasLayer _dbgUi;
	private TileMapLayer _positiveTileMap;
	private TileMapLayer _negativeTileMap;
	private Label _playerHealthLabel;
	private Array<Enemy> _enemies = new();

	private void InstantiateChildNodes()
	{
		_player = GetNode<CharacterBody2D>("Player");
		_camera = GetNode<Camera2D>("Player/PlayerCamera");
		_dbgUi = GetNode<CanvasLayer>("DBG_Info");
		_polaritySwitchCountdown = GetNode<Timer>("PolaritySwitchCountdown");
		_positiveTileMap = GetNode<TileMapLayer>("PositiveTileMap");
		_negativeTileMap = GetNode<TileMapLayer>("NegativeTileMap");
		_playerHealthLabel = GetNode<Label>("PlayerHealthLabel");
	}

	public override void _Ready()
	{
		GD.Print("Main ready");
		InstantiateChildNodes();
		SetCameraLimits();

		if (!OS.IsDebugBuild()) _dbgUi.Hide();

		_camera.MakeCurrent();

		for (var _ = 0; _ < EnemyCount; ++_) SpawnEnemy();
		
		_polaritySwitchCountdown.Start(PolaritySwitchDuration);
		_playerHealthLabel.SetText(200.ToString());
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
		
		_enemies.Add(enemy);
	}
	
	private void SetCameraLimits()
	{
		var mapLimits = _positiveTileMap.GetUsedRect();
		var mapCellSize = _positiveTileMap.TileSet.TileSize;
		
		_camera.LimitLeft = mapLimits.Position.X * mapCellSize.X;
		_camera.LimitRight = mapLimits.End.X * mapCellSize.X;
		_camera.LimitTop = mapLimits.Position.Y * mapCellSize.Y;
		_camera.LimitBottom = mapLimits.End.Y * mapCellSize.Y;
	}

	private void OnPlayerShoot(PackedScene bullet, Vector2 pos, Vector2 dir)
	{
		var b = (Bullet)bullet.Instantiate();
		AddChild(b);
		b.Start(pos, dir);
	}

	private void OnPolaritySwitchCountdownTimeout()
	{
		CurrentPolarity = CurrentPolarity == Polarity.Positive ? Polarity.Negative : Polarity.Positive;
		EmitSignal("PolaritySwitch", (int)CurrentPolarity);
		
		switch (CurrentPolarity)
		{
			case Polarity.Positive:
				_positiveTileMap.Visible = false;
				_negativeTileMap.Visible = true;
					
				foreach (var t in _enemies)
				{
					t.SetSpeed(1);
					// if enemy is too close, push back
					if (t.Position.DistanceTo(_player.Position) < 500)
					{
						GD.Print("TOO CLOSE");
						t.Position += (t.GlobalPosition - _player.GlobalPosition).Normalized() * 100;
					}
				}
				
				break;
			case Polarity.Negative:
				_positiveTileMap.Visible = true;
				_negativeTileMap.Visible = false;
				GD.Print("NEGATIVE POLARITY");
				
				foreach (var t in _enemies)
				{
					t.SetSpeed(400);
					
					// if enemy is close to player, reduce speed
					if (t.GlobalPosition.DistanceTo(_player.GlobalPosition) < 500)
					{
						// move away from player
						t.Position += (t.GlobalPosition - _player.GlobalPosition).Normalized() * 100;
							
						t.SetSpeed(35);
					}
				}
				
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		EmitSignal("PolaritySwitch", (int)CurrentPolarity);
		_polaritySwitchCountdown.Start(PolaritySwitchDuration);
	}

	public override void _UnhandledKeyInput(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			var pauseScene = GD.Load<PackedScene>("res://scenes/ui/PauseScreen.tscn");
			var pauseMenu = pauseScene.Instantiate();
			AddChild(pauseMenu);
		}
	}
}
