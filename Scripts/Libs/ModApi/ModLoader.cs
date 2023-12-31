﻿
namespace Scripts.Libs.ModApi
{
	/// <summary>
	/// Class that describes mod loading logic.
	/// </summary>
	public static class ModLoader
	{
		/// <summary>
		/// During the pre-initialization stage:<br/>
		///		- Scans the mods folder<br/>
		///		- Reads mod data<br/>
		///		- Creates Mod objects if the mod is valid<br/>
		///		- Reads the list of active mods and adds them to the list in the specified load order<br/>
		///		- Forms the list of active and inactive mods<br/>
		///		- Calls PreInit() on the initializers
		/// </summary>
		public static void PreInit()
		{
		}

		/// <summary>
		///		During the initialization stage, for each mod:<br/>
		///		- Entities (content) are created<br/>
		///		- Init() is called on the initializers
		/// </summary>
		public static void Init()
		{
		}


		/// <summary>
		///		During the post-initialization stage, additional patches are applied to dependent mods.
		///		Also calls PostInit() on the initializers.
		/// </summary>
		public static void PostInit()
		{
		}
	}
}
