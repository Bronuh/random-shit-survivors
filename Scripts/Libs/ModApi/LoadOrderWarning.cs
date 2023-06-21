
namespace Scripts.Libs.ModApi
{
	/// <summary>
	/// This class represents loading misorders for the specified mod.
	/// </summary>
	public class LoadOrderWarning
	{
		/// <summary>
		/// Reference to the current mod.
		/// </summary>
		public ModInfo Mod { get; private set; } = null;

		/// <summary>
		/// List of mods that must be loaded before the current mod, but loaded after.
		/// </summary>
		public List<ModInfo> ModsToBeLoadedBefore { get; private set; } = new();

		/// <summary>
		/// List of mods that must be loaded after the current mod, but loaded before.
		/// </summary>
		public List<ModInfo> ModsToBeLoadedAfter { get; private set; } = new();

		/// <summary>
		/// List of active mods that are incompatible with the current mod.
		/// </summary>
		public List<ModInfo> IncompatibleWithMods { get; private set; } = new();

		/// <summary>
		/// List of mod IDs that were not found in the active mods list.
		/// </summary>
		public List<string> RequiredModsNotFound { get; private set; } = new();

		/// <summary>
		/// This constructor will find all the mods misorders for the specified mod.
		/// </summary>
		/// <param name="mod">The mod to check for misorders.</param>
		/// <param name="activeMods">The list of active mods.</param>
		public LoadOrderWarning(ModInfo mod, List<ModInfo> activeMods)
		{
			ScanOrder(mod, activeMods);
		}

		/// <summary>
		/// This constructor will find all the mods misorders for the specified mod.
		/// </summary>
		/// <param name="mod">The mod to check for misorders.</param>
		/// <param name="activeMods">The list of active mods.</param>
		public LoadOrderWarning(ModBundle mod, List<ModBundle> activeMods)
		{
			ScanOrder(mod.Info, activeMods.Select(activeMod => activeMod.Info).ToList());
		}

		private void ScanOrder(ModInfo mod, List<ModInfo> activeMods)
		{
			Mod = mod;

			if (!activeMods.Contains(mod))
				throw new InvalidOperationException("The specified mod is not found in the list of active mods.");

			// Position of the current mod in the mods loading order.
			int modPosition = activeMods.IndexOf(mod);

			// Find all the incompatible mods
			IncompatibleWithMods = activeMods
				.Where(otherMod => mod.IncompatibleMods.Contains(otherMod.ModId))
				.ToList();

			// Find all required mods that is not present in the list
			RequiredModsNotFound = mod.RequiredMods
				.Where(modId => !activeMods.Select(otherMod => otherMod.ModId).Contains(modId))
				.ToList();

			// Find all the mods that must be loaded before the current mod, but currently loading after it.
			ModsToBeLoadedBefore = activeMods
				.Where(otherMod => mod.LoadBefore.Contains(otherMod.ModId) && activeMods.IndexOf(otherMod) > modPosition)
				.ToList();

			// Find all the mods that must be loaded after the current mod, but currently loading before it.
			ModsToBeLoadedAfter = activeMods
				.Where(otherMod => mod.LoadAfter.Contains(otherMod.ModId) && activeMods.IndexOf(otherMod) < modPosition)
				.ToList();
		}


		/// <summary>
		/// Checks if there is any misorders in the load order
		/// </summary>
		/// <returns>true if there is at least one any misorder</returns>
		public bool HasWarnings()
		{
			return
				IncompatibleWithMods.Count > 0
				|| RequiredModsNotFound.Count > 0
				|| ModsToBeLoadedAfter.Count > 0
				|| ModsToBeLoadedBefore.Count > 0;
		}
	}
}
