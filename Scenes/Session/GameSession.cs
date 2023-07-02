using Godot;
using Scripts.Libs.SaveLoad;
using System;

[GlobalClass]
public partial class GameSession : Node2D, IExposable
{
	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}


	public override void _PhysicsProcess(double delta)
	{
		// Not needed for now
	}


	public void ExposeData()
	{
		if (SaveLoad.Mode is SaveLoadMode.PostLoading)
		{

		}
	}
}
