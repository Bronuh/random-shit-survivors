namespace Scripts.Current;

public static class GameControls
{
	// Directions
	public const string KeyRight = "right";
	public const string KeyLeft = "left";
	public const string KeyUp = "up";
	public const string KeyDown = "down";

	// Other
	public const string KeySpace = "space";
	public const string KeyShift = "shift";
	public const string KeyCtrl = "control";

	// Mouse
	public const string KeyMouseLeft = "lmb";
	public const string KeyMouseMiddle = "mmb";
	public const string KeyMouseRight = "rmb";
	public const string WheelDown = "wheel_down";
	public const string WheelUp = "wheel_up";

	// Letters
	public const string KeyE = "e";
	public const string KeyF = "f";
	public const string KeyQ = "q";
	public const string KeyR = "r";

	// Functional
	public const string KeyF1 = "f1";
	public const string KeyF2 = "f2";
	public const string KeyF3 = "f3";

}

public static class GameNodes
{
	// Node names
	public const string MainNodeName = "MainNode";

	public const string UiNodeName = "%UiNode";
	public const string HudNodeName = "%HudNode";
	public const string MenuNodeName = "%MenuNode";
	public const string ShadersNodeName = "%ShadersNode";

	public const string WorldNodeName = "%WorldNode";
	public const string PhysicsSmoothingName = "%PhysicsSmoothing";
}

public static class GamePaths
{
	// Relative pathes
	public const string ModsRoot = "Mods"; // Directory where mods is located, relative to game execution directory.
	public const string GameDataRoot = "GameData";
	public const string ActiveModsFile = "activeMods.json";


	public const string AboutDir = "About"; // Must contain modinfo.json and optional icon.png.
	public const string ModInfoFile = "modinfo.json";
	public const string IconFile = "icon.png";
	public const string AssembliesDir = "Assemblies"; // Non-core assemblies must be located here.
	public const string AssetsDir = "Assets"; // All assets (images, textures, sounds, etc) must be located here.
	public const string DefsDir = "Defs"; // All game object definitions (serialized data) must be located here.
	public const string PatchesDir = "Patches"; // All Defs patches must be located here.
	public const string CoresDir = "Cores"; // All core assemblies must be located here. The core assemblies will be loaded and executed before all other mods.
}