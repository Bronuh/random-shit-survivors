namespace Scripts.Current;

public static class GameSettings{
    // General
	public static bool Debug = true;

	// Mod API Settings
	public static bool EnableModApi = true;
	public static bool TryRestoreModName = true; // Tries to restore the mod name from its directory name.
	public static bool TryRestoreModId = true; // Tries to restore an invalid mod ID using the author and mod name.
	public static bool TryRestoreAuthor = true; // Allows to use a 'Generic' author for the mods without author.
	public static bool AllowCoreMods = true; // Allows the execution of core assemblies on startup.

}