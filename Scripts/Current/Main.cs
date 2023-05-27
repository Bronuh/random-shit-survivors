using Godot;
using System;
using Scellecs.Morpeh;
using Scripts.Common.ModApi;
using Scripts.Common;

public partial class Main : Node2D
{
	private ModsManager _modsManager;
	private ModLoader _modLoader;
	private CoreModLoader _coreModLoader;

	public Main():base()
	{
		_coreModLoader = new CoreModLoader(this);
		_coreModLoader.Load();
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
