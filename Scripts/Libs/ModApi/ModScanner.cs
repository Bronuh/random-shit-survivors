using Godot;
using Newtonsoft.Json;
using Scripts.Current;
using static Scripts.Common.GamePaths;

namespace Scripts.Libs.ModApi;

internal static class ModScanner
{
	/// <summary>
	///		True
	/// </summary>
	public static bool ScanFinished { get; private set; } = false;



	private static List<ModBundle> _bundles = new();

	/// <summary>
	/// 	Starts scanning process. This method also will scan GameData directory.
	/// </summary>
	internal static void ScanMods()
	{
		// Check root directories
		Print("Current path: " + Path.GetFullPath("."));
		Directory.CreateDirectory(GameDataRoot);
		Directory.CreateDirectory(ModsRoot);

		// Check Game data directories
		string coreDir = Path.Combine(GameDataRoot, "Core");

		// Basically game is just one large mod
		Directory.CreateDirectory(coreDir);

		Directory.CreateDirectory(Path.Combine(coreDir, AboutDir));
		Directory.CreateDirectory(Path.Combine(coreDir, AssembliesDir));
		Directory.CreateDirectory(Path.Combine(coreDir, AssetsDir));
		Directory.CreateDirectory(Path.Combine(coreDir, DefsDir));
		Directory.CreateDirectory(Path.Combine(coreDir, CoresDir));

		// Now scan base game and possible DLCs as mods
		foreach (var dir in Directory.GetDirectories(GameDataRoot))
		{
			// Note that scanning order doesn't matter. Core game and DLCs will be sorted later automatically.
			// Obviously, for automatic sorting, DLCs (Downloadable Content) must have 'Core' listed as a dependency in modinfo.json.
			ScanMod(dir);
		}

		foreach (var dir in Directory.GetDirectories(ModsRoot))
		{
			// Note that scanning order doesn't matter. Core game and DLCs will be sorted later automatically.
			// Obviously, for automatic sorting, DLCs (Downloadable Content) must have 'Core' listed as a dependency in modinfo.json.
			ScanMod(dir);
		}

		ScanFinished = true;
	}



	public static List<ModBundle> GetModBundles()
	{
		if (!ScanFinished)
			throw new InvalidOperationException("Tried to get mod bundles, but scanning is not finished.");

		return _bundles;
	}



	/// <summary>
	/// 	Scans specified mod directory.
	/// </summary>
	private static void ScanMod(string path)
	{
		ModBundle bundle = new ModBundle();
		ModInfo info = null;
		try
		{
			info = JsonConvert.DeserializeObject<ModInfo>(File.ReadAllText(Path.Combine(path, AboutDir, ModInfoFile)));
		}
		catch (Exception e)
		{
			Err(e.Message);
			Err(e.StackTrace);
		}

		info ??= new ModInfo();

		bundle.SetInfo(info);
		bundle.SetPath(path);

		bool isValidBundle = ValidateModBundle(bundle);

		// Find all the core assemblies
		try
		{
			var coreModFiles = Directory.GetFiles(Path.Combine(path, CoresDir)).Where(f => f.GetExtension().Equals("dll"));
			foreach (var file in coreModFiles)
			{
				bundle.AddCoreAssembly(file);
			}
		}
		catch (Exception e)
		{
			// just write debug message
			Debug($"Can't load CORE assemblies for {bundle.Info.ModName}:");
			Debug(e.Message);
		}


		// Find all other assemblies
		try
		{
			var modFiles = Directory.GetFiles(Path.Combine(path, AssembliesDir)).Where(f => f.GetExtension().Equals("dll"));
			foreach (var file in modFiles)
			{
				bundle.AddAssembly(file);
			}
		}
		catch (Exception e)
		{
			// just write debug message
			Debug($"Can't load mod assemblies for {bundle.Info.ModName}:");
			Debug(e.Message);
		}

		_bundles.Add(bundle);

		// TODO: Resolve patches
	}



	private static bool ValidateModBundle(ModBundle bundle)
	{
		// The method will return true until something goes wrong during validation.
		// This is done on purpose so that all errors found during validation are logged.
		bool isValid = true;

		// First, restore things that can be restored
		// Try to restore mod author
		if (string.IsNullOrWhiteSpace(bundle.Info.ModName) && GameSettings.TryRestoreAuthor)
		{
			Print($"{bundle.ModPath}:\nModInfo does not contain an Author. Restoring with 'Generic' author.");
			bundle.Info.Author = "Generic";
		}

		// Try to restore mod name
		if (string.IsNullOrWhiteSpace(bundle.Info.ModName) && GameSettings.TryRestoreModName)
		{
			Print($"{bundle.ModPath}:\nModInfo does not contain a ModName. Restoring from directory name.");
			bundle.Info.ModName = Path.GetDirectoryName(bundle.ModPath);
		}


		// And then validate
		// Bundle must contain a ModInfo instance
		if (bundle.Info is null)
		{
			Err($"{bundle.ModPath}:\nMod bundle does not contain a ModInfo instance");
			isValid = false;
		}

		// And that ModInfo must have an ID.
		if (string.IsNullOrWhiteSpace(bundle.Info.ModId))
		{
			Err($"{bundle.ModPath}:\nModInfo does not contain a ModId");

			if (GameSettings.TryRestoreModId) bundle.Info.RestoreModId();
			else isValid = false;
		}


		// The ID must have at least two parts separated by a dot: author/team/organization/namespace and mod name.
		if (bundle.Info.ModId.Split('.').Length < 2)
		{
			Err($"{bundle.ModPath}:\nModInfo contain invalid ModId ({bundle.Info.ModId}).\n" +
			$"The ID must have at least two parts separated by a dot: author/team/organization/namespace and mod name.");

			if (GameSettings.TryRestoreModId) bundle.Info.RestoreModId();
			else isValid = false;
		}

		return isValid;
	}


	private static void RestoreModId(this ModInfo info)
	{
		info.ModId = $"{info.Author.Clear()}.{info.ModName.Clear()}";
		Print($"ModInfo does not contain a ModId. Restoring ModId from Author and ModName.");
	}
}
