﻿using Scripts.Common.EventApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.ModApi
{
	public static class ModApiEvents
	{
		/// <summary>
		/// Fired after loading core mods and before initializing mods manager.<br/>
		/// Mods manager will aquire the same mods list as in this event. <br/>
		/// </summary>
		/// <remarks>
		/// Cancel this event to prevent default ModsManager from starting.
		/// You still can use ModScanner.GetModBundles() to use with custom mods manager
		/// </remarks>
		public class ModsManagerAboutToStartEvent : CancellableMessage { }

		/// <summary>
		/// Fired after loading core mods and before initializing mods manager.<br/>
		/// Mods manager will aquire the same mods list as in this event. <br/>
		/// </summary>
		/// <remarks>
		/// Cancel this event to prevent default ModsManager from starting.
		/// You still can use ModScanner.GetModBundles() to use with custom mods manager
		/// </remarks>
		public class ModLoaderAboutToStartEvent : CancellableMessage { }
	}
}