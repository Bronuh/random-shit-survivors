using VirtualFileSystem;
using static Scripts.Current.GamePaths;

namespace Scripts.Libs.ModApi
{
	/// <summary>
	/// Provides functionality for managing storage and loading assets.
	/// </summary>
	public static class Storage
	{
		/// <summary>
		/// Gets the virtual file system manager for accessing assets.
		/// </summary>
		public static VFSManager Assets { get; private set; }

		/// <summary>
		/// Loads assets from active mods and adds them to the virtual file system.
		/// </summary>
		public static void LoadAssets()
		{
			Assets = new VFSManager();
			var modBundles = ModsManager.ActiveMods;
			foreach (var mod in modBundles)
			{
				// Combine the mod's path and the assets directory path
				string path = Path.Combine(mod.ModPath, AssetsDir);

				// If the directory doesn't exist, skip this mod
				if (!Directory.Exists(path))
					return;

				// Add the root container for the assets directory to the virtual file system
				Assets.AddRootContainer(path);
			}
		}
	}
}
