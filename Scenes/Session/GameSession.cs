using Godot;
using Scripts.Common.GodotNodes;
using Scripts.Current;
using Scripts.Current.GameTypes;
using Scripts.Libs;
using Scripts.Libs.SaveLoad;
using System.Collections.ObjectModel;

[GlobalClass]
public partial class GameSession : Node2D, IExposable
{
	// Getters for the session objects
	public static GameSession Instance => _instance;
	public static Entity Player
	{
		get
		{
			return Instance.GetNode<Entity>("Entities/Player");
		}
	}
	public static Node2D World => Instance.GetNode<Node2D>("Entities");
	public static FollowingCamera Camera => Instance.GetNode<FollowingCamera>("PlayerCamera");
	public static ReadOnlyCollection<Entity> Enemies => Instance._enemies.AsReadOnly();

	// Enemy scenes
	public static PackedScene BaseEnemy { get; private set; }
	public static PackedScene FastTriangle { get; private set; }
	public static PackedScene HeavySquare { get; private set; }
	public static PackedScene PowerfulHexagon { get; private set; }

	// Some values
	public static double Difficulty => BaseDifficulty + _passedTime / (_difficultyIncreaseMinutes * 60);
	public static int MaximumEnemies => (int)(EnemiesPerDifficultyLevel * Difficulty);

	public static double BaseDifficulty = 0.3;
	public static int EnemiesPerDifficultyLevel = 30;
	public static double EnemiesFillTime = 10;

	public static bool IsInProcess => Instance._isInProcess;

	private List<Entity> _enemies = new List<Entity>();
	private Entity _player = null;
	private static GameSession _instance;

	private static double _passedTime = 0;
	private static double _difficultyIncreaseMinutes = 10;

	// scenes paths
	private static string _baseEnemyPath = "res://Scenes/Entities/Enemies/BaseEnemy.tscn";
	private static string _fastTrianglePath = "res://Scenes/Entities/Enemies/FastTriangle.tscn";
	private static string _heavySquarePath = "res://Scenes/Entities/Enemies/HeavySquare.tscn";
	private static string _powerfulHexagonPath = "res://Scenes/Entities/Enemies/PowerfulHexagon.tscn";
	private List<PackedScene> _scenes = new List<PackedScene>();
	private WeightedRandomPicker<PackedScene> _picker = new();

	// time between spawns
	private double TimeBetweenSpawns => EnemiesFillTime / MaximumEnemies;
	private double _spawnThreshold = 0;
	private bool _isInProcess = true;


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

		// List of used enemy scenes
		_scenes.Add(BaseEnemy);
		_scenes.Add(FastTriangle);
		_scenes.Add(HeavySquare);
		_scenes.Add(PowerfulHexagon);

		// Weighted selector for the enemies
		_picker.Add(BaseEnemy, 100);
		_picker.Add(FastTriangle, 50);
		_picker.Add(HeavySquare, 25);
		_picker.Add(PowerfulHexagon, 50);

		// Get Player reference
		Instance._player = Instance.GetNode<Entity>("Entities/Player");

		Player.Init(PlayerData.Instance);
		Camera.TargetNode = Player;
	}

	public override void _Process(double delta)
	{
		_passedTime += delta;
		if (Instance._enemies.Count < MaximumEnemies)
		{
			_spawnThreshold += delta;

			int accumulatedSpawns = (int)(_spawnThreshold / TimeBetweenSpawns);

			for (int i = 0; i < accumulatedSpawns; i++)
			{
				// SpawnEnemy(_scenes.GetRandom());
				SpawnEnemy(_picker.PickRandom());
			}

			_spawnThreshold -= accumulatedSpawns * TimeBetweenSpawns;
		}

		MonitorLabel.SetGlobal("IsInProcess", IsInProcess);
		MonitorLabel.SetGlobal("Difficulty", Difficulty.ToString("F3"));
		MonitorLabel.SetGlobal("Enemies count", $"{Instance._enemies.Count}/{MaximumEnemies}");
		MonitorLabel.SetGlobal("Time between spawns", TimeBetweenSpawns.ToString("F1"));
	}
	
	// Spawn enemy outside the player's vision
	public static Entity SpawnEnemy(PackedScene enemyScene)
	{
		if (!IsInProcess)
			return null;

		Entity enemy = enemyScene.Instantiate<Entity>();
		enemy.Controller = new PrimitiveAiController();
		World.AddChild(enemy);
		enemy.Position = Player.Position + Rand.UnitVector2 * GetCameraRadius() * 1.5f;
		Instance._enemies.Add(enemy);
		enemy.DeathCallback = (e) =>
		{
			enemy.QueueFree();
			Instance._enemies.Remove(enemy);
		};
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

	public static void PlaySoundAt(string path, Vector2 position)
	{
		var soundPlayer = new AudioStreamPlayer2D();
		soundPlayer.Stream = GD.Load<AudioStream>(path);
		Instance.AddChild(soundPlayer);
		soundPlayer.Position = position;
		soundPlayer.Finished += () => soundPlayer.QueueFree();
		soundPlayer.Play();
	}

	public static void PlaySoundAt(IEnumerable<string> paths, Vector2 position)
	{
		PlaySoundAt(paths.GetRandom(), position);
	}


	public static float GetCameraRadius()
	{
		return GetVisibleRect().GetCircumradius();
	}

	public override void _PhysicsProcess(double delta)
	{
		// Not needed for now
	}

	// Is it even needed?
	public void ExposeData()
	{
		if (SaveLoad.Mode is SaveLoadMode.PostLoading)
		{

		}
	}

	public static Entity FindClosestEnemy(Vector2 position)
	{
		return Enemies.OrderBy(e => e.Position.DistanceTo(position)).FirstOrDefault();
	}

	public static void ShowDamage(Damage damage, Vector2 position)
	{
		var indicator = new DamageIndicator(damage);
		Instance.AddChild(indicator);
		indicator.Position = position;
	}
}
