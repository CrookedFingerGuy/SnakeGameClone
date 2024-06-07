using Godot;
using System;

public partial class SnakeTail : Node2D
{
	public Vector2 facing;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		facing= new Vector2(-1,0);
		GlobalPosition-=facing*128;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
