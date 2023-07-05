using Godot;

namespace Scripts.Current;

public static class InternalGameSettings{
    // General
	public static bool Debug = true;
	public static bool UseConstantSeed = true;

	// Mod API Settings
	public static bool EnableModApi = false;
	public static bool TryRestoreModName = true; // Tries to restore the mod name from its directory name.
	public static bool TryRestoreModId = true; // Tries to restore an invalid mod ID using the author and mod name.
	public static bool TryRestoreAuthor = true; // Allows to use a 'Generic' author for the mods without author.
	public static bool AllowCoreMods = true; // Allows the execution of core assemblies on startup.

	// Visuals
	public static Color PlayerColor = Grayscale(1);
	public static Color EnemyColor = Grayscale(0);

	// Gameplay
	public static double BaseDifficulty = 0;
	public static int EnemiesPerDifficultyLevel = 30;
	public static double DefenseArmor = 0.06; // Warcraft 3 style of damage calculation

	public static int MaxLevel = 100;
	public static int XpConst = 100;
}