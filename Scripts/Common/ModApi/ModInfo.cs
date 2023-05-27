using Godot;
using Lombok.NET;
using System;

namespace Scripts.Common.ModApi;

/// <summary>
///		This class represents a mod description.
///		Users should not create instances of this class manually.
/// </summary>
public partial class ModInfo
{
	public string ModName { get; internal set; }
	public string ModDescription { get; internal set; }
	public string[] SupportedVersions { get; internal set; }
	public string ModId { get; internal set; }
	public string Author { get; internal set; }

	

	/// <summary>
	///		The parameterless constructor is used ONLY for deserialization.
	/// </summary>
	[Obsolete]
	public ModInfo() { }



	// TODO: Must find out if it even needed
	/*public ModInfo(string modName, string modDescription, string[] supportedVersions, string modId, string author)
	{
		ModName = modName;
		ModDescription = modDescription;
		SupportedVersions = supportedVersions;
		ModId = modId;
		Author = author;
	}*/
}
