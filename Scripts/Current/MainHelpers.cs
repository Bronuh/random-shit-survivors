using Godot;
using Scripts.Common.EventApi;
using Scripts.Common.ModApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Scripts.Common.ModApi.ModApiEvents;


public partial class Main
{
	private void ScanMods()
	{
		ModScanner.ScanMods();
	}


	private void LoadCoreMods()
	{
		CoreModLoader.Initialize(this);
		CoreModLoader.Load();
	}

	private void ManageMods()
	{
		var manageEvent = new ModsManagerAboutToStartEvent();
		EventBus.Publish(manageEvent);

		if (manageEvent.IsCancelled) 
			return;

		ModsManager.Initialize();
	}

	private void LoadMods()
	{
		var modsLoadEvent = new ModLoaderAboutToStartEvent();
		EventBus.Publish(modsLoadEvent);

		if (modsLoadEvent.IsCancelled) 
			return;

		ModLoader.PreInit();
		ModLoader.Init();
		ModLoader.PostInit();
	}
}
