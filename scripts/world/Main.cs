using Godot;

namespace reversal.scripts.world;

public partial class Main : Node2D
{
	private CharacterBody2D _player;
	private Camera2D _camera;
	
	public override void _Ready()
	{
		InstantiateNodes();
		
		_camera.MakeCurrent();
	}
	
	/// <summary>
	/// Instantiates all the child nodes
	/// </summary>
	public void InstantiateNodes()
	{
		_player = GetNode<CharacterBody2D>("Player");
		_camera = GetNode<Camera2D>("Player/Camera2D");
	}
}
