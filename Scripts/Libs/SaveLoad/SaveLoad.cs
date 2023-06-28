
using Newtonsoft.Json.Linq;

namespace Scripts.Libs.SaveLoad
{
	/// <summary>
	/// Flags for the SaveLoad stages
	/// </summary>
	public enum SaveLoadMode
	{
		/// <summary>
		/// SaveLoad does nothing
		/// </summary>
		Idle,

		/// <summary>
		/// SaveLoad currently saving things
		/// </summary>
		Saving,

		/// <summary>
		/// SaveLoad currently saving things
		/// </summary>
		Loading,

		/// <summary>
		/// SaveLoad finished loading things and now you can run restoration logic
		/// </summary>
		PostLoading
	}

	/// <summary>
	/// Class for saving and loading the data.
	/// </summary>
	public static class SaveLoad
	{
		public static readonly string IsNullLabel = "isNull";
		public static readonly string TypePropertyName = "exposableType";

		public static Saver Saver { get; private set; } = new Saver();
		public static Loader Loader { get; private set; } = new Loader();
		public static SaveLoadMode Mode { get; internal set; } = SaveLoadMode.Idle;
		public static JObject CurrentObject
		{
			get
			{
				if (Mode is SaveLoadMode.Loading)
					return Loader.CurrentObject;

				if (Mode is SaveLoadMode.Saving)
					return Saver.CurrentObject;

				return null;
			}
		}

		/// <summary>
		/// Forces current saving/loading process to stop.
		/// </summary>
		public static void Stop()
		{
			Mode = SaveLoadMode.Idle;
			Saver.Stop();
			Loader.Stop();
		}


		public static bool EnterObject(string name)
		{
			if (Mode == SaveLoadMode.Idle) return false;

			if (Mode is SaveLoadMode.Saving)
			{
				return Saver.EnterObject(name);
			}

			if (Mode is SaveLoadMode.Loading or SaveLoadMode.PostLoading)
			{
				return Loader.EnterObject(name);
			}

			return true;
		}

		public static void ExitObject() 
		{
			if (Mode is SaveLoadMode.Saving)
			{
				Saver.ExitObject();
			}

			if (Mode is SaveLoadMode.Loading or SaveLoadMode.PostLoading)
			{
				Loader.ExitObject();
			}
		}
	}
}
