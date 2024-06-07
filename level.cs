using Godot;
using System;


public partial class level : Node2D
{
	public int score=0;
	private CustomSignals customSignals;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		customSignals=GetNode<CustomSignals>("/root/CustomSignals");
		customSignals.AppleEaten += HandleAppleEaten;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void _on_left_wall_body_entered(Node n)
	{
		Fruit temp=(Fruit)GetNode("Fruit");
		temp.customSignals.AppleEaten-=HandleAppleEaten;
		temp.QueueFree();
		GetTree().CallDeferred("change_scene_to_file","res://game_over_screen.tscn");
	}
	public void HandleAppleEaten()
	{
		score++;
		GetTree().CallGroup("ui", "UpdateScore", score);
	}
}
