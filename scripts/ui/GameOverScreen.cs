using Godot;

namespace reversal.scripts.ui;

public partial class GameOverScreen : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnRestartPress()
	{
		GetTree().ChangeSceneToFile("res://scenes/levels/Main.tscn");
	}

	private void OnQuitPress()
	{
		GetTree().Quit();
	}
}
