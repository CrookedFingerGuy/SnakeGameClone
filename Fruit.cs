using Godot;
using System;

public partial class Fruit : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public Vector2 location; //X 0-15 || Y 0-6
	private Random rand;

	public CustomSignals customSignals;
	public override void _Ready()
	{
		customSignals=GetNode<CustomSignals>("/root/CustomSignals");
		customSignals.AppleEaten += HandleAppleEaten;
		rand=new Random();
		location=new Vector2(3,2);
		GlobalPosition=new Vector2(location.X*64+80,location.Y*64+115);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void HandleAppleEaten()
	{
		GlobalPosition=new Vector2((rand.Next(0,15)*64+80),rand.Next(0,6)*64+115);
		GD.Print("Eaten"); 
	}

}
