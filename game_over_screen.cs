using Godot;
using System;

public partial class game_over_screen : Control
{
	//PackedScene levelScene=(PackedScene)GD.Load("res://level.tscn");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("space"))
		{
			GetTree().CallDeferred("change_scene_to_file","res://level.tscn");
		}
	}
}
