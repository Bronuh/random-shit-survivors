
namespace Scripts.Common.ModApi;

internal static class ModScanner
{
	// TODO: Define relative paths for scanner
	private const string ModsRoot = "Mods"; // Directory where mods is located, relative to game execution directory.


	private const string AboutDir = "About"; // Must contain modinfo.json and optional icon.png.
	private const string ModInfoFile = "modinfo.json";
	private const string IconFile = "icon.png";
	private const string AssembliesDir = "Assemblies"; // Non-core assemblies must be located here.
	private const string AssetsDir = "Assets"; // All assets (images, textures, sounds, etc) must be located here.
	private const string DefsDir = "Defs"; // All game object definitions (serialized data) must be located here.
	private const string PatchesDir = "Patches"; // All Defs patches must be located here.
	private const string CoresDir = "Cores"; // All core assemblies must be located here. The core assemblies will be loaded and executed before all other mods.
	

	
	/// <summary>
	/// 	Starts scanning process
	/// </summary>
	internal static void ScanMods(){
		
	}

	/// <summary>
	/// 	Scans specified mod directory
	/// </summary>
	private static void ScanMod(string path){

	}


	/// <summary>
	/// 	Collect mod data
	/// </summary>
	/// <param name="path">Path to the mod</param>
	private static ModInfo PrepareModInfo(string path){
		throw new NotImplementedException();
	}

	private static bool ValidateModInfo(ModInfo info){
		throw new NotImplementedException();
	}
}
