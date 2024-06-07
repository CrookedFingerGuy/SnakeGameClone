using Godot;
using System;
using System.Net.Http;

public partial class snake : Node2D
{
	private Vector2 facing;
	private bool growing;
	private Vector2 inputDirection;
	private int numOfBodySegments;
	private CustomSignals customSignals;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		customSignals=GetNode<CustomSignals>("/root/CustomSignals");
		facing= new Vector2(-1,0);
		growing=false;
		numOfBodySegments=1;
		//inputDirection=new Vector2(0,1);		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		inputDirection=Input.GetVector("left","right","up","down");
	}



	public void _on_timer_timeout()
	{
		//GD.Print(facing);
		Vector2 checkForBadTurn=inputDirection-facing; //Don't want 180 degree turns only 90 degree turns acceptable
		Node2D head=(Node2D)GetNode("SnakeHead");


		if((inputDirection.X!=0||inputDirection.Y!=0)&&(Math.Abs(checkForBadTurn.X)<=1&&Math.Abs(checkForBadTurn.Y)<=1)&&inputDirection!=facing)
		{
			//Change direction and move one space into the new facing direction
			if(growing)
				GrowSnake(inputDirection,head);
			else
				TurnSnake(inputDirection,head);

			growing=false;
			head.GlobalPosition+=inputDirection*64;
			facing=inputDirection;
		}
		else
		{	
			//No valid input. Move one space in the facing direction
			if(growing)
				GrowSnakeNoTurn(head);		
			else
				MoveSnakeNoTurn(head);
			growing=false;
			head.GlobalPosition+=facing*64;
		}

		Fruit fruit=(Fruit)GetParent().GetNode("Fruit");
		if(fruit.GlobalPosition==head.GlobalPosition)
		{
			growing=true;
			customSignals.EmitSignal(nameof(CustomSignals.AppleEaten));
		}
	}
	public void GrowSnakeNoTurn(Node2D head)
	{
		BodySegment lastBodySegment=(BodySegment)GetNode("BodySegment"+numOfBodySegments.ToString());
		/*
		SnakeTail tail=(SnakeTail)GetNode("SnakeTail");
		tail.GlobalPosition=lastBodySegment.GlobalPosition;
		if(lastBodySegment.facing.X==-1)//left
		{				
			tail.GlobalRotationDegrees=0.0f;
		}
		else if(lastBodySegment.facing.X==1)//right
		{
			tail.GlobalRotationDegrees=180.0f;
		}
		else if(lastBodySegment.facing.Y==-1)//up
		{
			tail.GlobalRotationDegrees=90.0f;
		}
		else if(lastBodySegment.facing.Y==1)//down
		{
			tail.GlobalRotationDegrees=-90.0f;
		}
		tail.facing=lastBodySegment.facing;
*/
		numOfBodySegments++;
		BodySegment temp=new BodySegment();
		BodySegment firstBodySegment=(BodySegment)GetNode("BodySegment1");
		temp.Name="BodySegment"+numOfBodySegments.ToString();
		Sprite2D s=new Sprite2D();
		s.Name="Sprite2D";
		temp.AddChild(s);
		this.AddChild(temp);
		if(numOfBodySegments==2)
		{
			temp.GlobalPosition=firstBodySegment.GlobalPosition;
			temp.GlobalRotationDegrees=firstBodySegment.GlobalRotationDegrees;
			temp.facing=firstBodySegment.facing;
			s.Texture=(firstBodySegment.GetNode("Sprite2D") as Sprite2D).Texture;
		}
		else
		{
			for(int i=numOfBodySegments-1;i>0;i--)
			{
				BodySegment prevBodySegment=(BodySegment)GetNode("BodySegment"+i.ToString());
				BodySegment nextBodySegment=(BodySegment)GetNode("BodySegment"+(i+1).ToString());
				(nextBodySegment.GetNode("Sprite2D") as Sprite2D).Texture=(prevBodySegment.GetNode("Sprite2D") as Sprite2D).Texture;
				nextBodySegment.GlobalPosition=prevBodySegment.GlobalPosition;
				nextBodySegment.GlobalRotationDegrees=prevBodySegment.GlobalRotationDegrees;
				nextBodySegment.facing=prevBodySegment.facing;
			}
		}	
		(firstBodySegment.GetNode("Sprite2D") as Sprite2D).Texture=(Texture2D)GD.Load("res://snakeBody.png");
		firstBodySegment.RotationDegrees=0;
		firstBodySegment.GlobalPosition=head.GlobalPosition;
		firstBodySegment.facing=facing;
		if(firstBodySegment.facing.Y==-1||firstBodySegment.facing.Y==1)
		{

			firstBodySegment.GlobalRotationDegrees=90.0f;
		}
		temp.Scale=head.Scale;		
	}
	public void MoveSnakeNoTurn(Node2D head)
	{
		BodySegment firstBodySegment=(BodySegment)GetNode("BodySegment1");
		SnakeTail tail=(SnakeTail)GetNode("SnakeTail");
		if(numOfBodySegments>1)
		{
			BodySegment lastBodySegment=(BodySegment)GetNode("BodySegment"+numOfBodySegments.ToString());
			tail.GlobalPosition=lastBodySegment.GlobalPosition;
			if(lastBodySegment.facing.X==-1)//left
			{				
				tail.GlobalRotationDegrees=0.0f;
			}
			else if(lastBodySegment.facing.X==1)//right
			{
				tail.GlobalRotationDegrees=180.0f;
			}
			else if(lastBodySegment.facing.Y==-1)//up
			{
				tail.GlobalRotationDegrees=90.0f;
			}
			else if(lastBodySegment.facing.Y==1)//down
			{
				tail.GlobalRotationDegrees=-90.0f;
			}
			tail.facing=lastBodySegment.facing;
			for(int i=numOfBodySegments-1;i>0;i--)
			{
				BodySegment prevBodySegment=(BodySegment)GetNode("BodySegment"+(i).ToString());
				BodySegment nextBodySegment=(BodySegment)GetNode("BodySegment"+(i+1).ToString());
				(nextBodySegment.GetNode("Sprite2D") as Sprite2D).Texture=(prevBodySegment.GetNode("Sprite2D") as Sprite2D).Texture;
				nextBodySegment.GlobalPosition=prevBodySegment.GlobalPosition;
				nextBodySegment.GlobalRotationDegrees=prevBodySegment.GlobalRotationDegrees;
				nextBodySegment.facing=prevBodySegment.facing;
			}
			firstBodySegment.GlobalPosition=head.GlobalPosition;
			Sprite2D img = (Sprite2D)firstBodySegment.GetNode("Sprite2D");
			img.Texture=(Texture2D)GD.Load("res://snakeBody.png");
			firstBodySegment.GlobalRotationDegrees=0;
			if(firstBodySegment.facing.Y==-1||firstBodySegment.facing.Y==1)
			{

				firstBodySegment.GlobalRotationDegrees=90.0f;
			}
		}
		else
		{			
			tail.GlobalPosition=firstBodySegment.GlobalPosition;
			if(firstBodySegment.facing.X==-1)//left
			{				
				tail.GlobalRotationDegrees=0.0f;
			}
			else if(firstBodySegment.facing.X==1)//right
			{
				tail.GlobalRotationDegrees=180.0f;
			}
			else if(firstBodySegment.facing.Y==-1)//up
			{
				tail.GlobalRotationDegrees=90.0f;
			}
			else if(firstBodySegment.facing.Y==1)//down
			{
				tail.GlobalRotationDegrees=-90.0f;
			}
			tail.facing=firstBodySegment.facing;
			firstBodySegment.GlobalPosition=head.GlobalPosition;
			Sprite2D img = (Sprite2D)firstBodySegment.GetNode("Sprite2D");
			img.Texture=(Texture2D)GD.Load("res://snakeBody.png");
			firstBodySegment.GlobalRotationDegrees=0;
			if(firstBodySegment.facing.Y==-1||firstBodySegment.facing.Y==1)
			{

				firstBodySegment.GlobalRotationDegrees=90.0f;
			}
		}

	}

	public void GrowSnake(Vector2 iDir, Node2D head)
	{
		BodySegment lastBodySegment=(BodySegment)GetNode("BodySegment"+numOfBodySegments.ToString());
		if(iDir.X==-1)//left
		{
			head.GlobalRotationDegrees=0.0f;
		}
		else if(iDir.X==1)//right
		{
			head.GlobalRotationDegrees=180.0f;
		}
		else if(iDir.Y==-1)//up
		{
			head.GlobalRotationDegrees=90.0f;
		}
		else if(iDir.Y==1)//down
		{
			head.GlobalRotationDegrees=-90.0f;
		}
		/*
		SnakeTail tail=(SnakeTail)GetNode("SnakeTail");
		tail.GlobalPosition=lastBodySegment.GlobalPosition;
		if(lastBodySegment.facing.X==-1)//left
		{				
			tail.GlobalRotationDegrees=0.0f;
		}
		else if(lastBodySegment.facing.X==1)//right
		{
			tail.GlobalRotationDegrees=180.0f;
		}
		else if(lastBodySegment.facing.Y==-1)//up
		{
			tail.GlobalRotationDegrees=90.0f;
		}
		else if(lastBodySegment.facing.Y==1)//down
		{
			tail.GlobalRotationDegrees=-90.0f;
		}
		tail.facing=lastBodySegment.facing;
*/
		numOfBodySegments++;
		BodySegment temp=new BodySegment();
		BodySegment firstBodySegment=(BodySegment)GetNode("BodySegment1");
		temp.Name="BodySegment"+numOfBodySegments.ToString();
		Sprite2D s=new Sprite2D();
		s.Name="Sprite2D";
		temp.AddChild(s);
		this.AddChild(temp);
		if(numOfBodySegments==2)
		{
			temp.GlobalPosition=firstBodySegment.GlobalPosition;
			temp.GlobalRotationDegrees=firstBodySegment.GlobalRotationDegrees;
			temp.facing=firstBodySegment.facing;
			s.Texture=(firstBodySegment.GetNode("Sprite2D") as Sprite2D).Texture;
		}
		else
		{
			for(int i=numOfBodySegments-1;i>0;i--)
			{
				BodySegment prevBodySegment=(BodySegment)GetNode("BodySegment"+i.ToString());
				BodySegment nextBodySegment=(BodySegment)GetNode("BodySegment"+(i+1).ToString());
				(nextBodySegment.GetNode("Sprite2D") as Sprite2D).Texture=(prevBodySegment.GetNode("Sprite2D") as Sprite2D).Texture;
				nextBodySegment.GlobalPosition=prevBodySegment.GlobalPosition;
				nextBodySegment.GlobalRotationDegrees=prevBodySegment.GlobalRotationDegrees;
				nextBodySegment.facing=prevBodySegment.facing;
			}
		}	
		(firstBodySegment.GetNode("Sprite2D") as Sprite2D).Texture=(Texture2D)GD.Load("res://snakeBodyTurn.png");
			switch(iDir.X)
			{
				case -1://going left
				{
					if(facing.Y==-1)
						firstBodySegment.GlobalRotationDegrees=-90.0f;
					else
						firstBodySegment.GlobalRotationDegrees=0.0f;
				}break;
				case 1://going right
				{
					if(facing.Y==-1)
						firstBodySegment.GlobalRotationDegrees=180.0f;
					else
						firstBodySegment.GlobalRotationDegrees=90.0f;

				}break;
				case 0://going up or down
				{
					if(iDir.Y==-1)
					{
						if(facing.X==-1)
							firstBodySegment.GlobalRotationDegrees=90.0f;
						else
							firstBodySegment.GlobalRotationDegrees=0.0f;
					}
					else
					{
						if(facing.X==-1)
							firstBodySegment.GlobalRotationDegrees=180.0f;
						else
							firstBodySegment.GlobalRotationDegrees=-90.0f;
					}
				}break;
			}

		firstBodySegment.facing=iDir;
		firstBodySegment.GlobalPosition=head.GlobalPosition;

		temp.Scale=head.Scale;		
		
	}

	public void TurnSnake(Vector2 iDir, Node2D head)
	{
		if(iDir.X==-1)//left
		{
			head.GlobalRotationDegrees=0.0f;
		}
		else if(iDir.X==1)//right
		{
			head.GlobalRotationDegrees=180.0f;
		}
		else if(iDir.Y==-1)//up
		{
			head.GlobalRotationDegrees=90.0f;
		}
		else if(iDir.Y==1)//down
		{
			head.GlobalRotationDegrees=-90.0f;
		}


		BodySegment firstBodySegment=(BodySegment)GetNode("BodySegment1");
		SnakeTail tail=(SnakeTail)GetNode("SnakeTail");


		if(numOfBodySegments>1)
		{
			BodySegment lastBodySegment=(BodySegment)GetNode("BodySegment"+numOfBodySegments.ToString());
			tail.GlobalPosition=lastBodySegment.GlobalPosition;
			if(lastBodySegment.facing.X==-1)//left
			{				
				tail.GlobalRotationDegrees=0.0f;
			}
			else if(lastBodySegment.facing.X==1)//right
			{
				tail.GlobalRotationDegrees=180.0f;
			}
			else if(lastBodySegment.facing.Y==-1)//up
			{
				tail.GlobalRotationDegrees=90.0f;
			}
			else if(lastBodySegment.facing.Y==1)//down
			{
				tail.GlobalRotationDegrees=-90.0f;
			}
			tail.facing=lastBodySegment.facing;
			for(int i=numOfBodySegments-1;i>0;i--)
			{
				BodySegment prevBodySegment=(BodySegment)GetNode("BodySegment"+(i).ToString());
				BodySegment nextBodySegment=(BodySegment)GetNode("BodySegment"+(i+1).ToString());
				(nextBodySegment.GetNode("Sprite2D") as Sprite2D).Texture=(prevBodySegment.GetNode("Sprite2D") as Sprite2D).Texture;
				nextBodySegment.GlobalPosition=prevBodySegment.GlobalPosition;
				nextBodySegment.GlobalRotationDegrees=prevBodySegment.GlobalRotationDegrees;
				nextBodySegment.facing=prevBodySegment.facing;
			}
		}
		else
		{
			tail.GlobalPosition=firstBodySegment.GlobalPosition;
			if(firstBodySegment.facing.X==-1)//left
			{				
				tail.GlobalRotationDegrees=0.0f;
			}
			else if(firstBodySegment.facing.X==1)//right
			{
				tail.GlobalRotationDegrees=180.0f;
			}
			else if(firstBodySegment.facing.Y==-1)//up
			{
				tail.GlobalRotationDegrees=90.0f;
			}
			else if(firstBodySegment.facing.Y==1)//down
			{
				tail.GlobalRotationDegrees=-90.0f;
			}
			tail.facing=firstBodySegment.facing;
		}
			Sprite2D img = (Sprite2D)firstBodySegment.GetNode("Sprite2D");
			img.Texture=(Texture2D)GD.Load("res://snakeBodyTurn.png");
			switch(iDir.X)
			{
				case -1://going left
				{
					if(facing.Y==-1)
						firstBodySegment.GlobalRotationDegrees=-90.0f;
					else
						firstBodySegment.GlobalRotationDegrees=0.0f;
				}break;
				case 1://going right
				{
					if(facing.Y==-1)
						firstBodySegment.GlobalRotationDegrees=180.0f;
					else
						firstBodySegment.GlobalRotationDegrees=90.0f;

				}break;
				case 0://going up or down
				{
					if(iDir.Y==-1)
					{
						if(facing.X==-1)
							firstBodySegment.GlobalRotationDegrees=90.0f;
						else
							firstBodySegment.GlobalRotationDegrees=0.0f;
					}
					else
					{
						if(facing.X==-1)
							firstBodySegment.GlobalRotationDegrees=180.0f;
						else
							firstBodySegment.GlobalRotationDegrees=-90.0f;
					}
				}break;
			}

		firstBodySegment.facing=iDir;
		firstBodySegment.GlobalPosition=head.GlobalPosition;
	}
}
