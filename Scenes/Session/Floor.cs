using Godot;
using System;

public partial class Floor : Node2D
{
	/// <summary>
	/// Used for terrain generation on-the-fly
	/// </summary>
	[Export]
	public Camera2D Camera { get; set; }


	public override void _Ready()
	{
		 
	}

	public override void _Process(double delta)
	{

	}

	public override void _PhysicsProcess(double delta)
	{

	}
}