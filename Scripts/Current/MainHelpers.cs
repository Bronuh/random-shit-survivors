using Scripts.Libs.EventApi;
using Scripts.Libs.ModApi;
using static Scripts.Libs.ModApi.ModApiEvents;

public partial class MainNode
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
