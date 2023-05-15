using Godot;
using System;

namespace BoilerplateProject.Scripts.Common.ModApi;

public struct ModInfo
{
	public string ModName { get; internal set; }
	public string ModDescription { get; internal set; }
	public string[] SupportedVersions { get; internal set; }
	public string ModId { get; internal set; }
	public string Author { get; internal set; }

	public ModInfo(string modName, string modDescription, string[] supportedVersions, string modId, string author)
	{
		ModName = modName;
		ModDescription = modDescription;
		SupportedVersions = supportedVersions;
		ModId = modId;
		Author = author;
	}
}
