using Godot;
using System;
using Scellecs.Morpeh;
using Scripts.Common.ModApi;
using Scripts.Common;
using Scripts.Current;

public partial class Main : Node2D
{
	private ModsManager _modsManager;
	private ModLoader _modLoader;
	private CoreModLoader _coreModLoader;

	public Main():base()
	{
		if (GameSettings.EnableModApi)
		{
			// Scan mods
			ModScanner.ScanMods();

			// Execute Core Mods
			_coreModLoader = new CoreModLoader(this);
			_coreModLoader.Load();

			// Manage mods
			// It's probably better to use hook for scanner or smth.
			_modsManager = new ModsManager();

			// Load mods
			_modLoader = new ModLoader();
		}
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
