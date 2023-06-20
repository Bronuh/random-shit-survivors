using Godot;
using Lombok.NET;
using System;

namespace Scripts.Common.ModApi;

/// <summary>
///		This class contains information about the mod.
///		Users should not create instances of this class manually.
/// </summary>
public class ModInfo
{
	// General info.
	public string ModName { get; internal set; } // Humen-friendly name of the mod.
	public string ModDescription { get; internal set; } // Mod description.
	public string[] SupportedVersions { get; internal set; } // What versions of the game are supported by the mod.
	public string ModId { get; internal set; } // Id of the mod. Consists of at least two parts, separated by the '.'
	public string Author { get; internal set; } // Name of the author of the mod.

	// Order resolving. These arrays must contain Ids of mods.
	public string[] IncompatibleMods { get; internal set; } // List of mods, that must not be loaded with current mod. The order doesn't matter.
	public string[] RequiredMods { get; internal set; } // List of mods, that must be just loaded with current mod. The order doesn't matter.
	public string[] LoadBefore { get; internal set; } // List of mods that must be loaded after this mod (load this mod before these).
	public string[] LoadAfter { get; internal set; } // List of mods that must be loaded before this mod (load this mod after these).

	/// <summary>
	///		The parameterless constructor is used mostly for deserialization.
	///		Users shouldn't use the constructor directly.
	/// </summary>
	public ModInfo() { }
}
