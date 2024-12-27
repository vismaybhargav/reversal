using Godot;
using System;

namespace reversal.scripts;

public partial class Main : Node2D
{
	private CharacterBody2D _player;
	
	public override void _Ready()
	{
		// INIT HERE
		_player = GetNode<CharacterBody2D>("Player");
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_accept"))
		{
			GD.Print("Accept button pressed");
		}
	}
}
