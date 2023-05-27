using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Common.ModApi
{
	/// <summary>
	///		Class responsible for user mod management.
	/// </summary>
	public class ModsManager
	{
		public List<Mod> ActiveMods { get; private set; }
		public List<Mod> InactiveMods { get; private set; }

		private bool _ready = false;

		public ModsManager() { }

		public virtual void Initialize(List<Mod> active, List<Mod> inactive) { }

		public virtual void CheckCompatibility()
		{

		}

		public virtual void CheckDependencies()
		{

		}

		public virtual void AutoOrderMods()
		{

		}
	}
}
