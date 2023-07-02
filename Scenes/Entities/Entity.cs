using Godot;
using Godot.Collections;
using Scripts.Common.GodotNodes;
using System;

public partial class Entity : Node2D
{
	[Export]
	public EntityController Controller { get; set; } = null;

	[Export]
	public Array<EntityComponent> Components { get; set; } = new Array<EntityComponent> ();

	[Export]
	public int MaxHP { get; set; }

	[Export]
	public double HP { get; set; }

	[Export]
	public double Armor { get; set; }

	[Export]
	public float Speed { get; set; } = 3000;


	public override void _Ready()
	{
		HP = MaxHP;
	}

	public override void _Process(double delta)
	{
		Position = Position + Controller.GetDirection() * Speed * (float)delta;
		MonitorLabel.SetGlobal("Position", Position);
		MonitorLabel.SetGlobal("Dir", Controller.GetDirection());
	}

	public override void _PhysicsProcess(double delta)
	{

	}
}
