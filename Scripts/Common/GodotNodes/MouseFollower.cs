using Godot;
using Scripts.Common;
using System;

public partial class MouseFollower : Node2D
{

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
		Position = AdvancedInputListener.MousePos();
	}

	public override void _PhysicsProcess(double delta)
	{

	}
}
