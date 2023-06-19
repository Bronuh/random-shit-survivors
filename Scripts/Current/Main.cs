using Godot;
using System;
using Scellecs.Morpeh;
using Scripts.Common.ModApi;
using Scripts.Common;
using Scripts.Current;
using Scripts.Common.EventApi;

public partial class Main : Node2D
{
	// Public getters for services
	public ModsManager ModsManager => _modsManager;
	public ModLoader ModLoader => _modLoader;

	private ModsManager _modsManager;
	private ModLoader _modLoader;

	public Main()
	{
		// Use Event API
		EventBus.Initialize(this);
		var _eventScanner = new EventScanner(_eventBus);

		if (GameSettings.EnableModApi)
		{
			// Scan mods
			ModScanner.ScanMods();
			ServiceStorage.ModsManager = new ModsManager();
			ServiceStorage.ModLoader = new ModLoader();

			// Execute Core Mods
			CoreModLoader.Initialize(this);
			CoreModLoader.Load();

			ServiceStorage.Lock();

			// Manage mods
			// It's probably better to use hook for scanner or smth.
			_modsManager = ServiceStorage.ModsManager;

			// Load mods
			_modLoader = ServiceStorage.ModLoader;
		}

		_eventScanner.ScanEventListeners();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ServiceStorage.Lock();
		_modLoader.PreInit();
		_modLoader.Init();
		_modLoader.PostInit();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{

	}
}
