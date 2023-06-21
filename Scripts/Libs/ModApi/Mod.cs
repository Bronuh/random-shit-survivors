using System.Reflection;

namespace Scripts.Libs.ModApi;

/// <summary>
///		Представляет ссылку на загруженный мод
/// </summary>
public class Mod
{
	public ModInfo ModInfo { get; private set; }

	public Assembly Assembly { get; private set; }
}

