using Godot;
using Scripts.Current;
using Scripts.Common.GodotNodes.UI;
using Scripts.Libs.EventApi;
using Scripts.Libs.ModApi;
using Esprima.Ast;

public partial class MainNode : Node2D
{
	public static MainNode Main { get; private set; }
	
	// TODO: Assign values on ready
	public WorldNode World { get; private set; }
	public HudNode Hud { get; private set; }
	public ShadersNode Shaders { get; private set; }
	public MenuNode Menu { get; private set; }


	public MainNode()
	{
		// Use Event API.
		EventBus.Initialize(this);
		Main = this;

		if (InternalGameSettings.EnableModApi)
		{
			// Scan mods, including cores.
			ScanMods();

			// Execute Core Mods.
			LoadCoreMods();

			// Locke services storage for writing.
			ServiceStorage.Lock();
			// Service storage now working in read-only mode.

			// Manage mods
			ManageMods();

			// Load mods
			LoadMods();
		}
		
		EventScanner.ScanEventListeners();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ServiceStorage.Lock();
		World = GetNode<WorldNode>(GameNodes.WorldNodeName);
		Hud = GetNode<HudNode>(GameNodes.HudNodeName);
		Shaders = GetNode<ShadersNode>(GameNodes.ShadersNodeName);
		Menu = GetNode<MenuNode>(GameNodes.MenuNodeName);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{

	}

	
}
