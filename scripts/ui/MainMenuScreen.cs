using Godot;

namespace reversal.scripts.ui;

public partial class MainMenuScreen : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnPlayButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/levels/Main.tscn");
	}
	
	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
