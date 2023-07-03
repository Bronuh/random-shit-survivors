using Godot;
using Scripts.Common.GodotNodes;
using Scripts.Current;
using Scripts.Libs.SaveLoad;
using System.Collections.ObjectModel;

[GlobalClass]
public partial class GameSession : Node2D, IExposable
{
	// Getters for the session objects
	public static Entity Player => Instance.GetNode<Entity>("Entities/Player");
	public static Node2D Playground => Instance.GetNode<Node2D>("Entities");
	public static ReadOnlyCollection<Entity> Enemies => Instance._enemies.AsReadOnly();
	public static FollowingCamera Camera => Instance.GetNode<FollowingCamera>("PlayerCamera");
	public static double Difficulty => 1 + _passedTime / (_difficultyIncreaseMinutes * 60);

	public static GameSession Instance => _instance ?? new GameSession();
	private static GameSession _instance;

	private List<Entity> _enemies = new List<Entity>();
	private Entity _player;

	private static double _passedTime = 0;
	private static double _difficultyIncreaseMinutes = 10;

	public GameSession()
	{
		_instance = this;
	}

	public override void _Ready()
	{
		Player.Init(PlayerData.Instance);
	}

	public override void _Process(double delta)
	{
		_passedTime += delta;
	}
	
	// Spawn enemy outside the player's vision
	public static Entity SpawnEnemy(PackedScene enemyScene)
	{
		Entity enemy = enemyScene.Instantiate<Entity>();
		Playground.AddChild(enemy);
		enemy.Position = Player.Position + RandNorm2() * GetCameraRadius() * 1.5f;

		return enemy;
	}

	// Get player's global vision rect
	public static Rect2 GetVisibleRect()
	{
		var viewport = Camera.GetViewportRect();
		var cameraSize = viewport.Size / Camera.Zoom;
		return new Rect2
		{
			Position = Camera.Position - cameraSize / 2,
			Size = cameraSize
		};
	}


	public static float GetCameraRadius()
	{
		return GetVisibleRect().GetCircumradius();
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
