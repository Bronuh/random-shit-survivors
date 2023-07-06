using Godot;
using Godot.Collections;
using Scripts.Common.GodotNodes;
using Scripts.Current;
using static GameSession;

public enum EntityTeam
{
	None,
	Player,
	Enemies
}

public partial class Entity : Node2D
{
	[Export]
	public EntityController Controller
	{
		get => _controller;
		set
		{
			_controller = value;
			_controller.Parent = this;
		}
	}

	[Export]
	public Array<EntityComponent> Components { get; set; } = new Array<EntityComponent> ();

	[Export]
	public int MaxHP { get; set; }

	[Export]
	public double HP { get; set; }

	[Export]
	public double Armor { get; set; }

	[Export]
	public double CollisionDamage { get; set; }

	[Export]
	public float Speed { get; set; } = 1000;

	[Export]
	public EntityTeam Team { get; set; } = EntityTeam.None;

	public Area2D CollisionArea => GetNode<Area2D>("Sprite2D/Area2D");

	public GameSession Session => GameSession.Instance;
	private EntityController _controller = null;

	public Action<Entity> DeathCallback = null;
	public Action<Entity> DamageCallback = null;

	public override void _Ready()
	{
		Controller.Parent = this;
		HP = MaxHP;
	}

	public override void _Process(double delta)
	{
		Position = Position + Controller.GetDirection() * Speed * (float)delta;
	}

	public override void _PhysicsProcess(double delta)
	{

	}

	internal void Init(PlayerData instance)
	{
		
	}
}
