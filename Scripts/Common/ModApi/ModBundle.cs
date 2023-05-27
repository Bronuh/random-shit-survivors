using System.Reflection;

namespace Scripts.Common.ModApi;

public sealed class ModBundle
{
	public ModInfo Info { get; private set; }
	public string ModPath { get; private set; }

	private List<Assembly> _assemblies = new();


    
    
}