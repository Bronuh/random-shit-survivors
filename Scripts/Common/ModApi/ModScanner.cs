
using Newtonsoft.Json;
using Scripts.Current;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;

namespace Scripts.Common.ModApi;

internal static class ModScanner
{
	/// <summary>
	///		True
	/// </summary>
	public static bool ScanFinished { get; private set; } = false;

	// Relative pathes for scanner
	private const string ModsRoot = "Mods"; // Directory where mods is located, relative to game execution directory.
	private const string GameDataRoot = "GameData";

	private const string AboutDir = "About"; // Must contain modinfo.json and optional icon.png.
	private const string ModInfoFile = "modinfo.json";
	private const string IconFile = "icon.png";
	private const string AssembliesDir = "Assemblies"; // Non-core assemblies must be located here.
	private const string AssetsDir = "Assets"; // All assets (images, textures, sounds, etc) must be located here.
	private const string DefsDir = "Defs"; // All game object definitions (serialized data) must be located here.
	private const string PatchesDir = "Patches"; // All Defs patches must be located here.
	private const string CoresDir = "Cores"; // All core assemblies must be located here. The core assemblies will be loaded and executed before all other mods.

	private static List<ModBundle> _bundles = new();

	/// <summary>
	/// 	Starts scanning process. This method also will scan GameData directory.
	/// </summary>
	internal static void ScanMods(){
		// Check root directories
		Directory.CreateDirectory(GameDataRoot);
		Directory.CreateDirectory(ModsRoot);

		// Check Game data directories
		string coreDir = Path.Combine(GameDataRoot,"Core");

		// Basically game is just one large mod
		Directory.CreateDirectory(coreDir);

		Directory.CreateDirectory(Path.Combine(coreDir,AboutDir));
		Directory.CreateDirectory(Path.Combine(coreDir,AssembliesDir));
		Directory.CreateDirectory(Path.Combine(coreDir,AssetsDir));
		Directory.CreateDirectory(Path.Combine(coreDir,DefsDir));

		// Now scan base game and possible DLCs as mods
		foreach(var dir in Directory.GetDirectories(GameDataRoot)){
			// Note that scanning order doesn't matter. Core game and DLCs will be sorted later automatically.
			// Obviously, for automatic sorting, DLCs (Downloadable Content) must have 'Core' listed as a dependency in modinfo.json.
			ScanMod(dir);
		}

		ScanFinished = true;
	}

	public static ReadOnlyCollection<ModBundle> GetModBundles() {
		return _bundles.AsReadOnly();
	}

	/// <summary>
	/// 	Scans specified mod directory.
	/// </summary>
	private static void ScanMod(string path){
		ModBundle bundle = new ModBundle();
		ModInfo info = null;
		try{
			info = JsonConvert.DeserializeObject<ModInfo>(File.ReadAllText(Path.Combine(path,AboutDir,ModInfoFile)));
		}
		catch(Exception e)
		{
			Err(e.Message);
			Err(e.StackTrace);
		}

		bundle.SetPath(path);

		bool isValidBundle = ValidateModBundle(bundle);

		// Find all the core assemblies
		var coreModFiles = Directory.GetFiles(Path.Combine(path,CoresDir)).Where(f => f.GetExtension().Equals("dll"));
		foreach(var file in coreModFiles)
		{
			bundle.AddCoreAssembly(file);
		}

		// Find all other assemblies
		var modFiles  = Directory.GetFiles(Path.Combine(path, AssembliesDir)).Where(f => f.GetExtension().Equals("dll"));
		foreach (var file in modFiles)
		{
			bundle.AddAssembly(file);
		}

		// TODO: Prepare VFS for definitions resolving
		// TODO: Resolve patches
	}


	
	private static bool ValidateModBundle(ModBundle bundle){
		// The method will return true until something goes wrong during validation.
		// This is done on purpose so that all errors found during validation are logged.
		bool isValid = true; 

		// First, restore things that can be restored
		// Try to restore mod author
		if (String.IsNullOrWhiteSpace(bundle.Info.ModName) && GameSettings.TryRestoreAuthor)
		{
			Print($"{bundle.ModPath}:\nModInfo does not contain an Author. Restoring with 'Generic' author.");
			bundle.Info.Author = "Generic";
		}

		// Try to restore mod name
		if (String.IsNullOrWhiteSpace(bundle.Info.ModName) && GameSettings.TryRestoreModName){
			Print($"{bundle.ModPath}:\nModInfo does not contain a ModName. Restoring from directory name.");
			bundle.Info.ModName = Path.GetDirectoryName(bundle.ModPath);
		}

		
		// And then validate
		// Bundle must contain a ModInfo instance
		if (bundle.Info is null){
			Err($"{bundle.ModPath}:\nMod bundle does not contain a ModInfo instance");
			isValid = false;
		}

		// And that ModInfo must have an ID.
		if(String.IsNullOrWhiteSpace(bundle.Info.ModId)){
			Err($"{bundle.ModPath}:\nModInfo does not contain a ModId");

			if (GameSettings.TryRestoreModId) RestoreModId(bundle.Info);
			else isValid = false;
		}


		// The ID must have at least two parts separated by a dot: author/team/organization/namespace and mod name.
		if(bundle.Info.ModId.Split('.').Length < 2){
			Err($"{bundle.ModPath}:\nModInfo contain invalid ModId ({bundle.Info.ModId}).\n"+
			$"The ID must have at least two parts separated by a dot: author/team/organization/namespace and mod name.");
			
			if (GameSettings.TryRestoreModId) RestoreModId(bundle.Info);
			else isValid = false;
		}

		return isValid;
	}

	private static void RestoreModId(this ModInfo info)
	{
		info.ModId = $"{info.Author}.{info.ModName}";
		Print($"ModInfo does not contain a ModId. Restoring ModId from Author and ModName.");
	}
}
