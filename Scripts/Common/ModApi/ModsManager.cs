using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Scripts.Common.GamePaths;

namespace Scripts.Common.ModApi
{
	/// <summary>
	///		Class responsible for user mod management.
	/// </summary>
	public static class ModsManager
	{
		// Active and Inactive mods already filled when AutoOrderMods is called.
		// Also, ActiveMods is already have previous load order.
		public static List<ModBundle> ActiveMods { get; private set; }
		public static List<ModBundle> InactiveMods { get; private set; }
		public static List<LoadOrderWarning> Warnings { get; private set; } = new List<LoadOrderWarning>();

		private static List<string> _activeModsList = new();

		// Already filled, when Initialize() is being called
		private static List<ModBundle> _allMods = new();
		private static bool _ready = false;


		public static void Initialize() {
			// Path to the activeMods.json
			string activeModsListPath = Path.Combine(ActiveModsFile);

			// Read list of Ids of the active mods from the file to _activeModsList using Newtonsoft.Json
			if (!File.Exists(activeModsListPath))
			{
				// If file doesn't exist, then _activeModsList must contain only 'Game.Core' mod.
				_activeModsList.Add("Game.Core");
				// Also, if file doesn't not exist, then it's need to be created.
				SaveActiveModsList(activeModsListPath);
			}
			else
			{
				LoadActiveModsList(activeModsListPath); // Load the activeMods.json file
			}

			// Then put all of the active mods to the ActiveMods.
			// Active mods must be sorted in the same order as in _activeModsList.
			ActiveMods = new List<ModBundle>();
			foreach (string activeModId in _activeModsList)
			{
				ModBundle bundle = _allMods.Find(mod => mod.Info.ModId == activeModId);

				if (bundle != null) ActiveMods.Add(bundle);
			}

			// All inactive mods must be listed in the InactiveMods, sorted by their names.
			InactiveMods = new List<ModBundle>();
			InactiveMods = _allMods.Except(ActiveMods).OrderBy(mod => mod.Info.ModName).ToList();

			// Check if current load order contains misordered entries
			CheckOrder();
		}

		public static void CheckOrder()
		{
			foreach (ModBundle bundle in ActiveMods)
			{
				var orderScan = new LoadOrderWarning(bundle, ActiveMods);
				if(orderScan.HasWarnings())
					Warnings.Add(orderScan);
			}
		}

		/// <summary>
		/// Automatically reorders ActiveMods according to their LoadBefore and LoadAfter lists in the Info.
		/// This method minimally affects the current order of the mods, sorting only those that are out of order.
		/// In addition, it is assumed that any mod must be loaded AFTER the "Game.Core" mod, unless otherwise explicitly stated.
		/// </summary>
		public static void AutoOrderMods()
		{

		}

		public static void RegisterMod(ModBundle bundle)
		{
			_allMods.Add(bundle);
		}


		private static void LoadActiveModsList(string activeModsListPath)
		{
			string json = File.ReadAllText(activeModsListPath);
			_activeModsList = JsonConvert.DeserializeObject<List<string>>(json);
		}

		private static void SaveActiveModsList(string activeModsListPath)
		{
			string json = JsonConvert.SerializeObject(_activeModsList, Formatting.Indented);
			File.WriteAllText(activeModsListPath, json);
		}
	}
}
