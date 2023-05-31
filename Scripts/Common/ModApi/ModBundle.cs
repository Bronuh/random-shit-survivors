using System.Collections.ObjectModel;
using System.Reflection;

namespace Scripts.Common.ModApi;

public sealed class ModBundle
{
	public ModInfo Info { get; private set; } = null;
	public string ModPath { get; private set; } = null;

	private List<string> _assemblies = new();
	private List<string> _coreAssemblies = new();

	public ModBundle(){}

	/// <summary>
	/// 	Defines path to this mod. It cannot be changed after the initial attachment.
	/// </summary>
	internal ModBundle SetPath(string path){
		ModPath ??= path;
		return this;
	}

	/// <summary>
	/// 	Attaches ModInfo to this bundle. It cannot be changed after the initial attachment.
	/// </summary>
	internal ModBundle SetInfo(ModInfo info){
		Info ??= info;
		return this;
	}

	/// <summary>
	/// 	Adds the specified path to the list of assemblies for this bundle.
	/// </summary>
	internal void AddAssembly(string path)
	{
		_assemblies.Add(path);
	}

	/// <summary>
	/// 	Adds the specified path to the list of core assemblies for this bundle.
	/// </summary>
	internal void AddCoreAssembly(string path)
	{
		_coreAssemblies.Add(path);
	}

	internal ReadOnlyCollection<string> GetCoreAssemblies()
	{
		return _coreAssemblies.AsReadOnly();
	}
}