using Godot;
using Scripts.Common.GodotNodes;
using Scripts.Current;
using Scripts.Libs.SaveLoad;
using System.Collections.ObjectModel;

[GlobalClass]
public partial class GameSession : Node2D, IExposable
{
	// Getters for the session objects
	public static GameSession Instance => _instance;
	public static Entity Player => Instance.GetNode<Entity>("Entities/Player");
	public static Node2D Playground => Instance.GetNode<Node2D>("Entities");
	public static FollowingCamera Camera => Instance.GetNode<FollowingCamera>("PlayerCamera");
	public static ReadOnlyCollection<Entity> Enemies => Instance._enemies.AsReadOnly();

	// Enemy scenes
	public static PackedScene BaseEnemy { get; private set; }
	public static PackedScene FastTriangle { get; private set; }
	public static PackedScene HeavySquare { get; private set; }
	public static PackedScene PowerfulHexagon { get; private set; }

	// Some values
	public static double Difficulty => InternalGameSettings.BaseDifficulty + _passedTime / (_difficultyIncreaseMinutes * 60);
	public static int MaximumEnemies => InternalGameSettings.EnemiesPerDifficultyLevel + (int)Mathf.Ceil(Difficulty);


	private List<Entity> _enemies = new List<Entity>();
	private Entity _player;
	private static GameSession _instance;

	private static double _passedTime = 0;
	private static double _difficultyIncreaseMinutes = 10;

	// scenes paths
	public static string _baseEnemyPath = "res://Scenes/Entities/Enemies/BaseEnemy.tscn";
	public static string _fastTrianglePath = "res://Scenes/Entities/Enemies/FastTriangle.tscn";
	public static string _heavySquarePath = "res://Scenes/Entities/Enemies/HeavySquare.tscn";
	public static string _powerfulHexagonPath = "res://Scenes/Entities/Enemies/PowerfulHexagon.tscn";


	public GameSession()
	{
		_instance = this;
	}

	public override void _Ready()
	{
		BaseEnemy = GD.Load<PackedScene>(_baseEnemyPath);
		FastTriangle = GD.Load<PackedScene>(_fastTrianglePath);
		HeavySquare = GD.Load<PackedScene>(_heavySquarePath);
		PowerfulHexagon = GD.Load<PackedScene>(_powerfulHexagonPath);

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
