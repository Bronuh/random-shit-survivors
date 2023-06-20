using Godot;
using System;
using Scellecs.Morpeh;
using Scripts.Common.ModApi;
using Scripts.Common;
using Scripts.Current;
using Scripts.Common.EventApi;

public partial class Main : Node2D
{
	public Main()
	{
		// Use Event API
		EventBus.Initialize(this);

		if (GameSettings.EnableModApi)
		{
			// Scan mods, including cores
			ModScanner.ScanMods();

			// Execute Core Mods
			CoreModLoader.Initialize(this);
			CoreModLoader.Load();

			ServiceStorage.Lock();

			// Manage mods
			ModsManager.Initialize();

			// Load mods
			ModLoader.PreInit();
		}

		EventScanner.ScanEventListeners();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ServiceStorage.Lock();
		ModLoader.Init();
		ModLoader.PostInit();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{

	}

	
}
