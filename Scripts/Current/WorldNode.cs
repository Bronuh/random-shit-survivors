using Scellecs.Morpeh;
using Godot;

public partial class WorldNode : Node2D
{
	public World World { get; private set; }
	public SystemsGroup Systems { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		World = World.Create();
		Systems = World.CreateSystemsGroup();
		
		Systems.Initialize();
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Called every physics tick. 'delta' is the elapsed time since the previous tick.
	public override void _PhysicsProcess(double delta)
	{
		
		World.Update((float)delta);
		World.FixedUpdate((float)delta);
		World.LateUpdate((float)delta);
		World.CleanupUpdate((float)delta);
	}

	public override void _ExitTree()
	{
		Systems?.Dispose();
		Systems = null;
		World?.Dispose();
		World = null;
	}
}
