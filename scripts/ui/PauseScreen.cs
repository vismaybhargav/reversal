using Godot;

namespace reversal.scripts.ui;

public partial class PauseScreen : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetTree().Paused = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnQuitButtonPressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://scenes/ui/MainMenuScreen.tscn");
	}
	
	private void OnContinueButtonPressed()
	{
		GetTree().Paused = false;
		QueueFree();
		GetTree().ChangeSceneToFile("res://scenes/levels/Main.tscn");
	}
}
