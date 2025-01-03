using Godot;
using reversal.scripts.entity;
using reversal.scripts.world;

namespace reversal.scripts.ui.dev_debug;
public partial class DebugInfo : CanvasLayer
{
	private Label _label;
	private Main _main;
	private Player _player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InstantiateNodes();
	}

	private void InstantiateNodes()
	{
		_label = GetNode<Label>("Label");
		_main = (Main)GetParent();
		_player = (Player)_main.GetChild(2);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_label.Text = $"""
		               FPS: {Engine.GetFramesPerSecond()}
		               Player Pos: {_player.Position}
		               Time until Switch: {Mathf.Round(((Timer)_main.GetChild(5)).TimeLeft * 100) / 100.0}
		               """;
	}

	// private static string MatchDev(string osName)
	// {
	// 	return osName switch
	// 	{
	// 		"Windows" => "Vismay",
	// 		"macOS" => "Jaewon",
	// 		"Linux" => "Dhruva",
	// 		_ => "Unknown"
	// 	};
	// }
}
