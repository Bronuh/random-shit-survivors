using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Libs.SaveLoad
{
	public enum SaveLoadMode
	{
		Idle,
		Saving,
		Loading,
		PostLoading
	}

	public static class SaveLoad
	{
		public static Saver Saver { get; private set; } = new Saver();
		public static Loader Loader { get; private set; } = new Loader();
		public static SaveLoadMode LoadMode { get; internal set; }
	}
}
